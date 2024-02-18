using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProxyDatabase
{
  
    public abstract class IDbManager
    {
      
        public SqlConnection connect;
        public SqlCommand cmd;
        public abstract void CreateTable(string tableName, string[] columns);
        public abstract void ViewTable(List<string> box, string TableName);
        public abstract void AllTables(ListBox box);
        public abstract void TableRecords(string tableName, DataGridView grid);
        public abstract void AddRecord(string table_name, List<string> fieldscontent);
        public abstract void UpdateRecord(string table_name, string columnName, string fieldscontent, string primarykey);
        public abstract void DeleteRecord(string table_name, string primarykey);
        public abstract string ViewPK(string TableName);
    }

    public class RealDbManager: IDbManager
    {
       public RealDbManager() {
            
            connect = new SqlConnection(@"Data Source=DESKTOP-9L7NQAF; Initial Catalog=MyDB;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = connect;
        }
        public override void CreateTable(string tableName, string[] columns)
        {
            try
            {
                cmd.CommandText = "create table " + tableName + "(";
                foreach (string s in columns)
                {
                    cmd.CommandText += s + ", ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ");";
                connect.Open();
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connect.Close();
            }
        }

        public override void ViewTable(List<string> box, string TableName)
        {
            box.Clear();
            cmd.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Add(reader.GetString(3));
            }
            reader.Close();
            connect.Close();
        }

        public override void AllTables(ListBox box)
        {
            box.Items.Clear();
            cmd.CommandText = "SELECT * FROM sys.Tables";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                box.Items.Add(reader.GetString(0));
            }
            reader.Close();
            connect.Close();
        }

        public override void TableRecords(string tableName, DataGridView grid)
        {
            try
            {
                connect.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", connect);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                grid.DataSource = dataSet.Tables[tableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connect.Close();
        }

        public override void AddRecord(string table_name, List<string> fieldscontent)
        {
            try
            {
                List<string> list = new List<string>();

                ViewTable(list, table_name);
                cmd.CommandText = $"INSERT INTO {table_name}(";

                foreach (string str in list)
                {
                    cmd.CommandText += str + ", ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ") ";
                cmd.CommandText += "VALUES(";
                foreach (string str in fieldscontent)
                {
                    cmd.CommandText += "'" + str + "', ";
                };
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 2);
                cmd.CommandText += ");";
                MessageBox.Show(cmd.CommandText);
                connect.Open();
                cmd.ExecuteNonQuery();
                connect.Close();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connect.Close();
                //return "";
            }

        }
        public override void DeleteRecord(string table_name, string primarykey)
        {
            try
            {

                cmd.CommandText = $"DELETE FROM {table_name} WHERE {ViewPK(table_name)} = {primarykey};";


                connect.Open();
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connect.Close();
        }
       
        public override void UpdateRecord(string table_name, string columnName, string fieldscontent, string primarykey)
        {
            try
            {
                cmd.CommandText = $"UPDATE {table_name} SET {columnName} = '{fieldscontent}'";
                cmd.CommandText += $" WHERE {ViewPK(table_name)} = {primarykey};";

                MessageBox.Show(cmd.CommandText);
                connect.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connect.Close();
        }

        public override string ViewPK(string TableName)
        {
            string str = "";
            cmd.CommandText = $"EXEC sp_pkeys @table_name = '{TableName}';";
            connect.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                str = reader.GetString(3);
            }
            reader.Close();
            connect.Close();
            return str;
        }
    }
    public  class ProxyDbManager : IDbManager
    {
        RealDbManager realDb;
       
        public ProxyDbManager(RealDbManager real)
        {
            this.realDb = real;

        }
        public override void CreateTable(string tableName, string[] columns) {
            if (tableName != "" && columns.Length > 0)
            {
                Console.WriteLine("Таблицю створено та додано до бази даних");
                this.realDb.CreateTable(tableName, columns);
            }
            else {
                MessageBox.Show("Неправильно заповнені дані в конструкторі, повторіть введення");
            }
        }
        public override void ViewTable(List<string> box, string TableName) {
            if (box != null && TableName!="")
                this.realDb.ViewTable(box, TableName);
        }
        public override void AllTables(ListBox box) {
            if(box!=null)
            this.realDb.AllTables(box);
        }
        public override void TableRecords(string tableName, DataGridView grid) {
            if(tableName!="" && grid!=null)
            this.realDb.TableRecords(tableName, grid);
        }
        public override void AddRecord(string table_name, List<string> fieldscontent) {
            if (table_name != "" && fieldscontent.Count>0)
                this.realDb.AddRecord(table_name, fieldscontent);
        }
        public override void UpdateRecord(string table_name, string columnName, string fieldscontent, string primarykey)
        {
            if (table_name != "" && columnName != "" && primarykey!="")
            this.realDb.UpdateRecord(table_name, columnName, fieldscontent, primarykey);
        }
        public override void DeleteRecord(string table_name, string primarykey) {
            if (table_name != "" && primarykey != "")
                this.realDb.DeleteRecord(table_name, primarykey);
        }
        public override string ViewPK(string TableName)
        {
            if (TableName != "")
            {
                return this.realDb.ViewPK(TableName);
            }
            else {
                return "";
            }
        }
    }
}
