using GasWeb.Domain.Exceptions;
using GasWeb.Domain.Infrastructure;
using GasWeb.Domain.Users.Entities;
using GasWeb.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GasWeb.Domain
{
    internal static class EntityFrameworkExtensions
    {
        private const int MaxPageSize = 200;

        public static async Task<T> GetAsync<T>(this DbSet<T> dbSet, params object[] keyValues) where T : class
        {
            return await dbSet.FindAsync(keyValues) ?? throw new EntityNotFound();
        }

        public static async Task<T> GetAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> selector) where T : class
        {
            return await queryable.FirstOrDefaultAsync(selector) ?? throw new EntityNotFound();
        }

        public static async Task<PageResponse<TResult>> PickPageAsync<T, TResult>(
            this IQueryable<T> query, 
            int pageNumber, 
            int pageSize, 
            Expression<Func<T, TResult>> map)
        {
            if (pageNumber < 1) throw new ArgumentException("Page number must be greater than zero", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page number must be greater than zero", nameof(pageNumber));

            if (pageSize > 200)
                pageSize = MaxPageSize;

            var offset = pageSize * (pageNumber - 1);
            var resultsTask = query.Select(map).Skip(offset).Take(pageSize).ToListAsync();
            var totalCountTask = query.LongCountAsync();

            await Task.WhenAll(new Task[] { resultsTask, totalCountTask });

            var pagingInfo = new PagingInfo(
                pageNumber: pageNumber,
                pageSize: pageSize,
                totalCount: totalCountTask.Result);
            
            return new PageResponse<TResult>(resultsTask.Result, pagingInfo);
        }

        public static ModelBuilder AuditEntity<T>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<T>> configuration)
            where T : AuditEntity
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            modelBuilder.Entity<T>(b =>
            {
                b.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedByUserId);
                b.HasOne<User>().WithMany().HasForeignKey(x => x.ModifiedByUserId);
                configuration(b);
            });

            return modelBuilder;
        }

        [SuppressMessage("Security", "EF1000:Possible SQL injection vulnerability.")]
        public static Task<int> UpsertAsync<T>(this DbContext dbContext, IEnumerable<T> entities, Action<SqlUpsertOptions<T>> upsertOptionsFactory)
        {
            var options = new SqlUpsertOptions<T>();
            upsertOptionsFactory(options);

            var tableName = typeof(T).Name;
            var updateColumns = options.Columns.Except(options.OnConflictColumns);

            var sql = $@"
                INSERT INTO ""{ tableName }""
                (
                    { options.Columns.GetColumnsList() }
                )
                VALUES
                    { string.Join(",", entities.Select(x => $"({ string.Join(",", options.ValuesSelector(x)) })")) }
                ON CONFLICT
                (
                    { options.OnConflictColumns.GetColumnsList() }
                )
                DO UPDATE SET
                    { string.Join(",", updateColumns.Select(c => $@"""{c}"" = EXCLUDED.""{c}""")) }";

            return dbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        private static string GetColumnsList(this IEnumerable<string> columnNames) => 
            string.Join(",", columnNames.Select(x => $@"""{x}"""));
    }
}
