using Microsoft.VisualStudio.TestTools.UnitTesting;
using GitVersionTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitVersionTool.Tests
{
    [TestClass()]
    public class TicketCommitTests
    {
        [TestMethod()]
        public void TicketCommitTest()
        {
            string message = "This test is linked to ticket #566, oh and #5098";
            TicketCommit ticket = new TicketCommit(message);
            List<int> expectedList = new List<int>();
            expectedList.Add(566);
            expectedList.Add(5098);
            string expected = expectedList.ToArray().ToString(); 
            List<int> actualList = ticket.GetTicketNumberList();
            string actual = actualList.ToArray().ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}