using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IStudentRepo
    {
        void Create(Student s);
        Student Get(int id);
        List<Student> Get();
        void Update(Student s);
        void Delete(int id);

    }
}
