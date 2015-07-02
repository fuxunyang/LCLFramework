﻿using System;
using System.ComponentModel.Design;
using System.Windows;
using EnvDTE;
using System.Linq;
using LCL.Data;

namespace LCL.VSPackage.Commands.MigrateOldDatabase
{
    /// <summary>
    /// 从旧数据库生成实体代码的命令。
    /// </summary>
    class MigrateOldDatabaseCommand : Command
    {
        public MigrateOldDatabaseCommand()
        {
            this.CommandID = new CommandID(GuidList.guidVSPackageCmdSet, PkgCmdIDList.cmdidMigrateOldDatabaseCommand);
        }

        protected override void OnQueryStatus()
        {
            this.MenuCommand.Enabled = this.GetSelectedProjects().Count == 1;
        }

        protected override void ExecuteCore()
        {
            var projects = this.GetSelectedProjects();
            if (projects.Count != 1)
            {
                MessageBox.Show("请选择要生成实体的项目，再执行本命令。");
                return;
            }

            var win = new MigerateDatabaseWizardWindow();
            win.txtConnectionString.Text = @"server=.\SQLExpress;database=XXX;uid=XXX;pwd=XXX";
            win.txtDomainName.Text = projects[0].Properties.Item("RootNamespace").Value.ToString();
            var res = win.ShowDialog();
            if (res == true)
            {
                var connectionString = win.txtConnectionString.Text;
                var domainName = win.txtDomainName.Text;

                //构造实体代码生成器。
                var generator = new CodeFileGenerator();
                generator.DomainName = domainName;
                generator.DbSetting = DbSetting.SetSetting(
                    domainName, connectionString,
                    DbSetting.Provider_SqlClient//目前只支持 SqlServer。
                    );

                //默认生成在 Entities 文件夹内。
                var items = projects[0].ProjectItems;
                generator.Directory = items.FindByName("Entities") ?? items.AddFolder("Entities");
                generator.RepoDirectory = items.FindByName("Repositories") ?? items.AddFolder("Repositories");

                //开始生成代码。
                try
                {
                    var error = generator.Generate();
                    var msg = string.Format("生成完毕，本次一共生成了 {0} 张表。", generator.SuccessCount);
                    if (!string.IsNullOrEmpty(error))
                    {
                        msg += Environment.NewLine + "无法移植的表：" + Environment.NewLine + error;
                    }
                    MessageBox.Show(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}