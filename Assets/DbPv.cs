using UnityEngine;
using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DbPv : MonoBehaviour

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

    private void Pv()
    {

        dbcmd = dbcon.CreateCommand();
        string sql =
            "SELECT pv_container.voltage, pv_container.energy, pv_container.power, pv_container.current, pyranometer.irradiance " +
            "FROM pv_container INNER JOIN pyranometer ON " +
            "pv_container.input_time = pyranometer.input_time ORDER BY pyranometer.input_time DESC LIMIT 1";
        dbcmd.CommandText = sql;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            var voltage = (double)reader["voltage"];
            var energy = (double)reader["energy"];
            var current = (double)reader["current"];
            var power = (double)reader["power"];
            var irradiance = (double)reader["irradiance"];
            
            Solar.text = ("Voltage     : " + voltage.ToString() + " V"+
                        "\nEnergy      : " + energy.ToString() + " kWh"+
                        "\nCurrent     : " + current.ToString() + " A"+
                        "\nPower       : " + power.ToString() + " W"+
                        "\nIrradiance : " + irradiance.ToString() + " W.m^−2"
                         );
            Debug.Log("Done?!");

            if (voltage==0 || energy==0 || current==0 || power==0|| irradiance==0)
            {
                Solar.text = ("PV mengalami gangguan, mohon lanjutkan ke Troubleshooting di menu panduan");
            }
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



