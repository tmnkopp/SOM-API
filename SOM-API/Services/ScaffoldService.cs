using SOMData;
using SOMData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOMAPI.Services
{
    public interface IScaffoldService
    {
        void Delete(Scaffold scaffold);
        IQueryable<Scaffold> GetAll();
        Scaffold GetById(int ScaffoldId);
        void Insert(Scaffold scaffold);
        void Update(Scaffold scaffold);
    }
    public class ScaffoldService : IScaffoldService
    {
        private readonly IRepository<Scaffold> _scaffoldRepository;
        public ScaffoldService()
        {
        }
        public ScaffoldService(IRepository<Scaffold> scaffoldRepository)
        {
            _scaffoldRepository = scaffoldRepository;
        }
        public virtual Scaffold GetById(int ScaffoldId)
        {
            Scaffold scaffold = _scaffoldRepository.GetById(ScaffoldId);
            return scaffold;
        }
        public virtual IQueryable<Scaffold> GetAll()
        {
            IQueryable<Scaffold> scaffolds = _scaffoldRepository.Table;
            return scaffolds;
        }
        public virtual void Insert(Scaffold scaffold)
        {
            if (scaffold == null)
                throw new ArgumentNullException("Scaffold");
            _scaffoldRepository.Insert(scaffold);
        }
        public virtual void Update(Scaffold scaffold)
        {
            if (scaffold == null)
                throw new ArgumentNullException("Scaffold");
            _scaffoldRepository.Update(scaffold);
        }
        public virtual void Delete(Scaffold scaffold)
        {
            if (scaffold == null)
                throw new ArgumentNullException("Scaffold");
            _scaffoldRepository.Delete(scaffold);
        }
    }

}