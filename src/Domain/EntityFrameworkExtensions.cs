﻿using GasWeb.Domain.Exceptions;
using GasWeb.Shared;
using Microsoft.EntityFrameworkCore;
using System;
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
            var resultsTask = query.Select(map).Take(pageSize).Skip(offset).ToListAsync();
            var totalCountTask = query.LongCountAsync();

            await Task.WhenAll(new Task[] { resultsTask, totalCountTask });

            var pagingInfo = new PagingInfo(
                pageNumber: pageNumber,
                pageSize: pageSize,
                totalCount: totalCountTask.Result);

            return new PageResponse<TResult>(resultsTask.Result, pagingInfo);
        }
    }
}
