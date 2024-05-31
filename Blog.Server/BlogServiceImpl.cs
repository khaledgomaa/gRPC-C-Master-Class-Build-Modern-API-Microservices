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

        public async override Task ListBlogs(ListBlogRequest request, IServerStreamWriter<ListBlogResponse> responseStream, ServerCallContext context)
        {
            var blogs = await _appDbContext.Blogs.ToListAsync();

            for (int i = 0; i < blogs.Count; i++)
            {
                await responseStream.WriteAsync(new() { Blog = Map(blogs[i]) });
            }
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

        private static Blog Map(Blogg.Dal.Entities.Blog blog)
        {
            return new()
            {
                Id = blog.Id,
                Title = blog.Title,
                AuthId = blog.AuthId,
                Content = blog.Content
            };
        }
    }
}
