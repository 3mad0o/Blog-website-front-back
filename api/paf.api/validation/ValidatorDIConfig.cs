using paf.api.validation.Blog;
using paf.api.validation.Comment;
using paf.api.validation.user;

namespace paf.api.validation
{
    public class ValidatorDIConfig
    {
        public static void RegisterValidatorDI(IServiceCollection services)
        {
            services.AddScoped<UserCreateValidatior>();
            services.AddScoped<UserUpdateValidator>();
            services.AddScoped<BlogCreateValidator>();
            services.AddScoped<BlogUpdateValidator>();
            services.AddScoped<CommentCreateValidator>();
            services.AddScoped<CommentUpdateValidator>();
        }
    }
}
