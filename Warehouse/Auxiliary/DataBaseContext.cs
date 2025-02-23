using SQLite;
using BaseElement = Warehouse.Models.BaseElement;

namespace Warehouse.Auxiliary
{
    public class DataBaseContext
    {
        private const string DB_NAME = "baseElements.bd";
        private readonly SQLiteAsyncConnection _connection;

        public DataBaseContext()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<BaseElement>();
        }

        public async Task<List<BaseElement>> GetElements()
        {
            return await _connection.Table<BaseElement>().ToListAsync(); 
        }

        public async Task<BaseElement> GetById(int id)
        {
            return await _connection.Table<BaseElement>().Where(x => x.id == id).FirstOrDefaultAsync();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-A8OL1B5\\SQLEXPRESS;Database=.NET MAUI;Integrated Security=True;");
        //}
    }
}
