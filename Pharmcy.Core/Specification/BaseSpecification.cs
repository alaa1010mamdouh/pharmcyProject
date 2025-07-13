using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } =  new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        //Get All
        public BaseSpecification()
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }

        //Get By Id

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy=orderBy;
        }


        public void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }


    }
}