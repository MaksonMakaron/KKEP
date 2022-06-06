using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubsystemKKEP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubsystemKKEP.Classes.Tests
{
    [TestClass()]
    public class PasswordLoginManagementTests
    {


        [TestMethod()]
        public void SearchLoginDontRepeat_ReturnFalse()
        {
            //Предусловие
            string login = "ITB";
            bool expected = false;

            //Действие
            bool actual = PasswordLoginManagement.SearchLoginDontRepeat(login);

            //Условие
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SearchLoginDontRepeat_ReturnTrue()
        {
            //Предусловие
            string login = "Voloshin";
            bool expected = true;

            //Действие
            bool actual = PasswordLoginManagement.SearchLoginDontRepeat(login);

            //Условие
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PasswordCheck_ReturnTrue()
        {
            //Предусловие
            string password = "ITB";
            bool expected = true;

            //Действие
            bool actual = PasswordLoginManagement.PasswordCheck(password);

            //Условие
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PasswordCheck_ReturnFalse()
        {
            //Предусловие
            string password = "123Ds";
            bool expected = false;

            //Действие
            bool actual = PasswordLoginManagement.PasswordCheck(password);

            //Условие
            Assert.AreEqual(expected, actual);
        }
    }
}