using HappyPath.Services.Data.Context;
using HappyPath.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Domain
{
    //Glue code to make a default service for working with type <TestItem>
    public interface ITestItemService : IBaseEntityService<TestItem> { }

    public class TestItemService : BaseEntityService<TestItem>, ITestItemService
    {
        public TestItemService(IHappyPathSession session) : base(session) { }
    }
}
