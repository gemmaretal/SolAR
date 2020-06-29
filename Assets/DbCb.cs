using UnityEngine;
using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class DbCb : MonoBehaviour

{
    
    public TextMeshProUGUI Pv;
    public TextMeshProUGUI Pln;
    public TextMeshProUGUI FC;
    public TextMeshProUGUI DC;
    public TextMeshProUGUI AC;
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

    private void Cb()
    {

        dbcmd = dbcon.CreateCommand();
        string sql =
            "select * from state order by input_time limit 5";

        dbcmd.CommandText = sql;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            var cb_pv = (string)reader["cb_pv"];
            var cb_pln = (string)reader["cb_pln"];
            var cb_fc = (string)reader["cb_fc"];
            var cb_dc_load = (string)reader["cb_dc_load"]; 
            var cb_ac_load = (string)reader["cb_ac_load"];

            if (cb_pv == "true")
            {
                Pv.text = ("ON");
            }
            else
            {
                Pv.text = ("OFF");
            }
            
            if (cb_pln == "true")
            {
               
                Pln.text = ("ON");
            }
            else
            {
                Pln.text = ("OFF");
            }

            if (cb_fc == "true")
            {
                
                FC.text = ("ON");
             }
            else
            {
                FC.text = ("OFF");
            }

            if (cb_dc_load == "true")
            {

                DC.text = ("ON");
            }
            else
            {
                DC.text = ("OFF");
            }

            if (cb_ac_load == "true")
            {
                AC.text = ("ON");
            }
            else
            {
                AC.text = ("OFF");
            }

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
            Cb();
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
                Cb();
                Routine();
            }
        }
    }
}



