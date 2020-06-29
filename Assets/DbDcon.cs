using UnityEngine;
using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DbDcon : MonoBehaviour

{
    public TextMeshProUGUI Solar;
    private int Counter = 0;
    NpgsqlDataReader reader;
    NpgsqlConnection dbcon;
    NpgsqlCommand dbcmd;
    private string connectionString =
         "Server=54.156.143.88;" +
         "Database=container;" +
         "User ID=postgres;" +
         "Password=vpp12345;";


    public void Start()
    {
        dbcon = new NpgsqlConnection(connectionString);
        dbcon.Open();
        Debug.Log("Execute dbcon.open");
        Routine();
    }

    private void Dcon()
    {

        dbcmd = dbcon.CreateCommand();
        string sql =
            "SELECT voltage, energy, power, current FROM dcon ORDER BY input_time DESC LIMIT 1";
        dbcmd.CommandText = sql;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            var voltage = (double)reader["voltage"];
            var energy = (double)reader["energy"];
            var current = (double)reader["current"];
            var power = (double)reader["power"];
            Solar.text = ("Voltage : " + voltage.ToString() +" V"+
                        "\nEnergy  : " + energy.ToString()+" kWh"+
                        "\nCurrent : " + current.ToString() +" A"+
                        "\nPower   : " + power.ToString() +" W");
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
            Dcon();
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
                Dcon();
                Routine();
            }
        }
    }
}



