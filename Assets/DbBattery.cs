using UnityEngine;
using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DbBattery : MonoBehaviour

{
    public TextMeshProUGUI Solar;
    private int Counter = 0;
    NpgsqlDataReader reader;
    NpgsqlConnection dbcon;
    NpgsqlCommand dbcmd;
    private string connectionString =
         "Server=54.145.67.197;" +
         "Database=battery;" +
         "User ID=postgres;" +
         "Password=vpp12345;";


    public void Start()
    {
        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();
        Debug.Log("Execute dbcon.open");
        Routine();
    }

    private void Pv()
    {

        dbcmd = dbcon.CreateCommand();
        string sql =
            "SELECT percentage.percentage, export.power FROM percentage " +
            "INNER JOIN export ON percentage.input_time = export.input_time ORDER BY percentage.input_time DESC LIMIT 1";
        dbcmd.CommandText = sql;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            var percentage = (double)reader["percentage"];
            var power = (double)reader["power"];
            
            Solar.text = ("Percentage : " + percentage.ToString("F") + " %" +
                     ("\nPower         : " + power.ToString() + " W"));
            Debug.Log("Done?!");
        }
    }


    public void Routine()
    {
        StartCoroutine(DoEveryFiveSeconds());
    }
    IEnumerator DoEveryFiveSeconds()
    {
        if (Counter == 0)
        {
            Counter++;
            yield return new WaitForSeconds(5f);
            Debug.Log("Test DB Counter : " + Counter);
            Pv();
            Routine();
        }
        else
        {
            if (!reader.IsClosed)
            {
                reader.Close();
                Counter++;
                yield return new WaitForSeconds(5f);
                Debug.Log("Test DB Counterz : " + Counter);
                Pv();
                Routine();
            }
        }
    }
}



