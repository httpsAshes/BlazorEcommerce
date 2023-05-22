namespace BlazorEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _Context;
        public CategoryService(DataContext context)
        {
            _Context = context;
        }
        public  async Task<ServiceResponse<List<Category>>> GetCategories()
        {
           var categories = await _Context.Categories.ToListAsync();
           return new ServiceResponse<List<Category>> 
           {
               Data = categories 
           };

        }
    }
}
