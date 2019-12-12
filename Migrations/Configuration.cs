namespace SubmitBug.Migrations
{
    using SubmitBug.BLL;
    using SubmitBug.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SubmitBug.DAL.SubBugEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SubmitBug.DAL.SubBugEntities context)
        {
            if (context.TB_UserAccess.Any()&&context.TB_LoginOn.Any())
            {
                return;
            }
            else
            {
                if (!context.TB_UserAccess.Any())
                {
                    List<TB_UserAccess> listUserAccess = new List<TB_UserAccess>()
                    {
                        new TB_UserAccess()
                        {
                            AccessName = "Users"
                        },
                        new TB_UserAccess()
                        {
                            AccessName = "PowerUsers"
                        },
                        new TB_UserAccess()
                        {
                            AccessName = "Follower"
                        }
                        ,
                        new TB_UserAccess()
                        {
                            AccessName = "Guest"
                        }
                    };
                    context.TB_UserAccess.AddRange(listUserAccess);
                    context.SaveChanges();
                }
                if (!context.TB_LoginOn.Any())
                {
                    MD5DataEncryption md5 = new MD5DataEncryption();
                    List<TB_LoginOn> listLoginOn = new List<TB_LoginOn>()
                    {
                        new TB_LoginOn()
                        {
                            LoginNo="Guest",
                            LoginName="Guest",
                            LoginPwd=md5.MD5Encrypt("123456"),
                            LPhone="1234567890",
                            ComputerNo="T",
                            AId=1
                        },
                        new TB_LoginOn()
                        {
                            LoginNo="Admin",
                            LoginName="Admin",
                            LoginPwd=md5.MD5Encrypt("12345678"),
                            LPhone="1234567890",
                            ComputerNo="T",
                            AId=2
                        }
                    };

                    context.TB_LoginOn.AddRange(listLoginOn);
                    context.SaveChanges();
                }
            }
        }
    }
}
