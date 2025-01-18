using AutoMapper;
using BLL.DTOs;
using DAL.EF;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class StudentService
    {
        //public static List<StudentDTO> Get()
        //{
        //    var repo = new StudentRepo();
        //    var data = repo.Get();
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Student, StudentDTO>();
        //    });
        //    var mapper = new Mapper(config);
        //    var ret = mapper.Map<List<StudentDTO>>(data);
        //    return ret;

        //}
        //public static void Create(StudentDTO s)
        //{
        //    // convert to EF model
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<StudentDTO, Student>();
        //    });
        //    var mapper = new Mapper(config);
        //    var data = mapper.Map<Student>(s);
        //    var repo = new StudentRepo();
        //    repo.Create(data);
        //}

        private static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Student, StudentDTO>().ReverseMap();
        });

        private static Mapper mapper = new Mapper(config);

        public static List<StudentDTO> Get()
        {
            var repo = new StudentRepo();
            var data = repo.Get();
            return mapper.Map<List<StudentDTO>>(data);
        }

        public static StudentDTO Get(int id)
        {
            var repo = new StudentRepo();
            var data = repo.Get(id);
            return mapper.Map<StudentDTO>(data);
        }

        public static void Create(StudentDTO s)
        {
            var repo = new StudentRepo();
            var entity = mapper.Map<Student>(s);
            repo.Create(entity);
        }

        public static void Update(StudentDTO s)
        {
            var repo = new StudentRepo();
            var entity = mapper.Map<Student>(s);
            repo.Update(entity);
        }


        public static void Delete(int id)
        {
            var repo = new StudentRepo();
            repo.Delete(id);
        }
    }
}
