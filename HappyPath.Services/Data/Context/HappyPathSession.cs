using HappyPath.Services.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Data.Context
{
    //Glue Code
    public interface IHappyPathSession : ISession { }

    public class HappyPathSession : Session, IHappyPathSession
    {
        public HappyPathSession() : base(new HappyPathDbContext()) { }
    }
}
