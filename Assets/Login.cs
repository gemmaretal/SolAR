using UnityEngine;
using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField uname;
    public TMP_InputField pass;
    private string test;
    private string test1;
    public TextMeshProUGUI test2;
    public void LoginDB()

    {
        string connectionString =
           "Server=54.156.143.88;" +
           "Database=userdb;" +
           "User ID=postgres;" +
           "Password=vpp12345;";
        NpgsqlConnection dbcon;

        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();
        NpgsqlCommand dbcmd = dbcon.CreateCommand();
        
        string sql =
            "SELECT*FROM login";
        dbcmd.CommandText = sql;

        NpgsqlDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            var username = (string)reader["username"];
            var password = (string)reader["password"];

            test = uname.text;
            test1 = pass.text;

            if (test == username && test1 == password)
            {
                Debug.Log("success");
                SceneManager.LoadScene("Main");
            }
            else if (test == username && test1 != password)
            {
                test2.text = ("Wrong Password");
            }
            else if (test != username && test1 == password)
            {
                test2.text = ("Wrong Username");
            }
            else
            {
                Debug.Log("wrong");
                test2.text = ("Wrong Password and Username");
            }

            
        }

        // clean up 
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }
}


