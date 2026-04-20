using EF_MultiTable.Models;
using EF_MultiTable.Services.Interfaces;
using EF_MultiTable.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EF_MultiTable.Services.Implementations
{
    public class StateService : IStateService
    {

        private readonly EfMultiTableContext _context;

        public StateService(EfMultiTableContext context)
        {
            _context = context;
        }

        public void Create(StateCityVM vm)
        {
            var state = new State
            {
                StateName = vm.StateName,
                Cities = vm.Cities.Select(c => new City
                {
                    CityName = c.CityName
                }).ToList()
            };

            _context.States.Add(state);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var state = _context.States
                        .Include(s => s.Cities)
                        .FirstOrDefault(s => s.StateId == id);
            _context.States.Remove(state);
            _context.SaveChanges();


        }

        public StateCityVM Get(int id)
        {
            var state = _context.States
                        .Include(s => s.Cities)
                        .FirstOrDefault(s => s.StateId == id);

            return new StateCityVM
            {
                StateId = state.StateId,
                StateName = state.StateName,
                Cities = state.Cities.Select(c => new CityVM
                {
                    CityId = c.CityId,
                    CityName = c.CityName
                }).ToList()
            };
        }

        public void Update(StateCityVM vm)
        {
            var state = _context.States
                        .Include(s => s.Cities)
                        .FirstOrDefault(s => s.StateId == vm.StateId);

            state.StateName = vm.StateName;

            var existingCityIds = state.Cities.Select(c => c.CityId).ToList();

            var vmCityIds = vm.Cities.Where(c => c.CityId != 0).Select(c => c.CityId).ToList();

            var citiesToDelete = state.Cities.Where(c => !vmCityIds.Contains(c.CityId)).ToList();
            foreach (var city in citiesToDelete)
            {
                state.Cities.Remove(city);
            }

            foreach (var cityVm in vm.Cities)
            {
                if (cityVm.CityId == 0)
                {
                    state.Cities.Add(new City
                    {
                        CityName = cityVm.CityName
                    });
                }
                else
                {
                    var existingCity = state.Cities.First(c => c.CityId == cityVm.CityId);
                    existingCity.CityName = cityVm.CityName;
                }
            }

            _context.States.Update(state);
            _context.SaveChanges();
        }


        public List<StateListVM> GetAll()
        {
            var states =  _context.States
                            .Include(s => s.Cities)
                            .ToList();

            return states.Select(s => new StateListVM
            {
                StateId = s.StateId,
                StateName = s.StateName,
                Cities = string.Join(", ", s.Cities.Select(c => c.CityName))
            }).ToList();
        }
    }
}
