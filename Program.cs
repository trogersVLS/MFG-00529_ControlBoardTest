/**********************************************************************************************
 * Program.cs
 * - Main Entry for Control Board Functional Test Program
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 **********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GPIO;
using System.IO.Ports;
using VLS;

namespace ControlBoardTest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //TestArea


            //Login users first
            var users = GetUsers();
            var login = new LoginForm(users);
            string user = login.ShowForm();
            login.Dispose();

            if(user != null)
            {
                if(user == "")
                {
                    user = "TEST";
                }
               Application.Run(new GUI_Main(user));
            }
           
        }

        private static XmlNode GetUsers()
        {
            //open the settings.xml file

            XmlDocument configuration = new XmlDocument();
            XmlNode user_ids = null;


            configuration.Load(@".\Configuration\settings.xml");


            foreach (XmlNode xml in configuration.DocumentElement.ChildNodes)
            {

                if (xml.Name == "users")
                {
                    user_ids = xml;
                }

            }

            return user_ids;

        }
    }
}