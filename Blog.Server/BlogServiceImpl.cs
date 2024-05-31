using Blogg.Dal;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using static Blog.BlogService;

namespace Blog.Server
{
    public class BlogServiceImpl : BlogServiceBase
    {
        private readonly AppDbContext _appDbContext;

        public BlogServiceImpl()
        {
            _appDbContext = new();
            _appDbContext.Database.Migrate();
        }

        public async override Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
        {
            var blog = Map(request.Blog);

            await _appDbContext.AddAsync(blog);

            await _appDbContext.SaveChangesAsync();

            request.Blog.Id = blog.Id;

            return new() { Blog = request.Blog };
        }

        private static Blogg.Dal.Entities.Blog Map(Blog blog)
        {
            return new()
            {
                Title = blog.Title,
                AuthId = blog.AuthId,
                Content = blog.Content
            };
        }
    }
}
