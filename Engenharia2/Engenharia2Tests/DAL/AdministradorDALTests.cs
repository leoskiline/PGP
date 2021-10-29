using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engenharia2.DAL.Tests
{
    [TestClass()]
    public class AdministradorDALTests
    {
        [TestMethod()]
        public void obterIdPorNomeTest()
        {
            AdministradorDAL admDal = new AdministradorDAL();
            int id = admDal.obterIdPorNome("Leonardo Custodio dos Santos");
            Assert.IsNotNull(id);
        }
    }
}