using DAL.EF;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class StudentRepo : Repo, IStudentRepo
    {
        //UMSContext db;
        //public StudentRepo()
        //{
        //    db = new UMSContext();
        //}
        //public void Update(Student s)
        //{
        //    var exobj = Get(s.StudentID);
        //    db.Entry(exobj).CurrentValues.SetValues(s);
        //    db.SaveChanges();
        //}

        //public Student Get(int id)
        //{
        //    return db.Students.Find(id);
        //}

        //public List<Student> Get()
        //{
        //    return db.Students.ToList();
        //}

        //public void Delete(int id)
        //{
        //    var exobj = Get(id);
        //    db.Students.Remove(exobj);
        //    db.SaveChanges();
        //}

        public void Create(Student s)
        {
            db.Students.Add(s);
            db.SaveChanges();
        }

        

        public void Update(Student s)
        {
            var exobj = db.Students.Find(s.StudentID);
            if (exobj != null)
            {
                db.Entry(exobj).CurrentValues.SetValues(s);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Student not found");
            }
        }

        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        public List<Student> Get()
        {
            return db.Students.ToList();
        }

        public void Delete(int id)
        {
            var exobj = db.Students.Find(id);
            if (exobj != null)
            {
                db.Students.Remove(exobj);
                db.SaveChanges();
            }
        }

    }
}
