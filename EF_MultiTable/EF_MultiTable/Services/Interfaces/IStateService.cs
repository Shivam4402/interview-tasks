using EF_MultiTable.ViewModels;

namespace EF_MultiTable.Services.Interfaces
{
    public interface IStateService
    {
        void Create(StateCityVM vm);
        StateCityVM Get(int id);
        void Update(StateCityVM vm);
        void Delete(int id);
        List<StateListVM> GetAll();
    }
}
