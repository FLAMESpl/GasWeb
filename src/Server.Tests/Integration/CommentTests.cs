using FluentAssertions;
using GasWeb.Shared;
using GasWeb.Shared.Comments;
using GasWeb.Shared.GasStations;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GasWeb.Server.Tests.Integration
{
    [Collection(IntegrationTestCollection.Name)]
    public class CommentTests
    {
        private readonly IntegrationTestFixture fixture;

        public CommentTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GasStation_AddComment_CommentIsAddedSuccessfully()
        {
            // given

            var gasStationId = await CreateGasStation();

            // when

            var addCommentModel = new AddCommentModel
            {
                Content = "Comment",
                SubjectId = gasStationId.ToString(),
                Tag = CommentTag.GasStation
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Comments.ToString(), addCommentModel);

            // then

            var comment = await fixture.HttpClient.GetJsonAsync<Comment>($"{ Routes.Comments }/{ createdAt.Id }");
            comment.Should().BeEquivalentTo(addCommentModel);
        }

        [Fact]
        public async Task SomeCommentsForGasStation_GetList_SomeCommentsAreRetrieved()
        {
            // given

            var gasStationId = await CreateGasStation();
            var addCommentModel = new AddCommentModel
            {
                Content = "Comment",
                SubjectId = gasStationId.ToString(),
                Tag = CommentTag.GasStation
            };

            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Comments.ToString(), addCommentModel);
            await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Comments.ToString(), addCommentModel);

            // when

            var url = $"{ Routes.Comments }?tag={ (int)CommentTag.GasStation }&subjectId={ gasStationId }";
            var comments = await fixture.HttpClient.GetJsonAsync<PageResponse<Comment>>(url);
            comments.Results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CommentForGasStation_Delete_CommentIsDeleted()
        {
            // given

            var gasStationId = await CreateGasStation();
            var addCommentModel = new AddCommentModel
            {
                Content = "Comment",
                SubjectId = gasStationId.ToString(),
                Tag = CommentTag.GasStation
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Comments.ToString(), addCommentModel);

            // when

            await fixture.HttpClient.DeleteAsync($"{ Routes.Comments }/{ createdAt.Id }");

            // then

            var response = await fixture.HttpClient.GetAsync($"{ Routes.PriceSubmissions }/{ createdAt.Id }");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CommentForGasStation_UpdateContent_UpdatedContentCanBeRetrieved()
        {
            // given

            var gasStationId = await CreateGasStation();
            var addCommentModel = new AddCommentModel
            {
                Content = "Comment",
                SubjectId = gasStationId.ToString(),
                Tag = CommentTag.GasStation
            };

            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.Comments.ToString(), addCommentModel);

            // when 

            var updateCommentModel = new UpdateCommentModel { Content = "Updated Comment" };
            await fixture.HttpClient.PutJsonAsync($"{ Routes.Comments }/{ createdAt.Id }", updateCommentModel);

            // then

            var comment = await fixture.HttpClient.GetJsonAsync<Comment>($"{ Routes.Comments }/{ createdAt.Id }");
            comment.Should().BeEquivalentTo(updateCommentModel);
        }

        private async Task<long> CreateGasStation()
        {
            var addModel = new AddGasStationModel { Name = "Amic Energy" };
            var createdAt = await fixture.HttpClient.PostJsonAsync<CreatedResponse>(Routes.GasStations.ToString(), addModel);
            return createdAt.Id;
        }
    }
}
