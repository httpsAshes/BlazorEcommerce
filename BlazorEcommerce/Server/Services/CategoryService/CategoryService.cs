namespace BlazorEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _Context;
        public CategoryService(DataContext context)
        {
            _Context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            _Context.Categories.Add(category);
            await _Context.SaveChangesAsync();  
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
          Category category = await GetCategoryById(id);
            if(category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            category.Deleted= true;
            await _Context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        private async Task<Category> GetCategoryById(int id)
        {
            return  await _Context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var categories = await _Context.Categories
               .Where(c => !c.Deleted )
               .ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public  async Task<ServiceResponse<List<Category>>> GetCategories()
        {
           var categories = await _Context.Categories
                .Where(c => !c.Deleted && c.Visible)
                .ToListAsync();
           return new ServiceResponse<List<Category>> 
           {
               Data = categories 
           };

        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var DbCategory = await GetCategoryById(category.Id);
            if(DbCategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found"
                };
            }

            DbCategory.Name= category.Name;
            DbCategory.Url= category.Url;
            DbCategory.Visible = category.Visible;

            await _Context.SaveChangesAsync();
            return await GetAdminCategories();
        }
    }
}
