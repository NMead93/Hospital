using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Hospital.Objects
{
  public class Patient
  {
    private int _id;
    private string _name;
    private string _birthday;
    private int doctor_id;

    public Patient (string name, string birthday, int doctorId,int Id=0)
    {
      _id = Id;
      _name = name;
      _birthday = birthday;
      doctor_id = doctorId;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void  SetName(string name)
    {
      _name = name;
    }
    public string GetBirthday()
    {
      return _birthday;
    }
    public void  SetBirthday(string birthday)
    {
      _birthday = birthday;
    }
    public int GetDoctorId()
    {
      return doctor_id;
    }
    public void SetDoctorId(int doctorId)
    {
      doctor_id = doctorId;
    }

    public override bool Equals(System.Object otherPatient)
    {
      if (!(otherPatient is Patient))
      {
        return false;
      }
      else
      {
        Patient newPatient = (Patient) otherPatient;
        bool idEquality = (this.GetId()== newPatient.GetId());

        return (idEquality);
      }
    }
    public static List<Patient> GetAll()
    {
      List<Patient> allPatients = new  List<Patient>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patients;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int patientId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string birthday = rdr.GetString(3);
        int doctorId = rdr.GetInt32(2);
        Patient newPatients = new Patient(name, birthday, doctorId, patientId);
        allPatients.Add(newPatients);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allPatients;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patients (name, doctor_id, birthday) OUTPUT INSERTED.id VALUES ( @PatientName,@PatientDoctorId,@PatientBirthday);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@PatientName";
      nameParameter.Value = this.GetName();

      SqlParameter birthdayParameter = new SqlParameter();
      birthdayParameter.ParameterName = "@PatientBirthday";
      birthdayParameter.Value = this.GetBirthday();

      SqlParameter patientDoctorIdParameter = new SqlParameter();
      patientDoctorIdParameter.ParameterName = "@PatientDoctorId";
      patientDoctorIdParameter.Value = this.GetDoctorId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(birthdayParameter);
      cmd.Parameters.Add(patientDoctorIdParameter);


      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Patient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patients WHERE id = @PatientId;", conn);
      SqlParameter patientIdParameter = new SqlParameter();
      patientIdParameter.ParameterName = "@PatientId";
      patientIdParameter.Value = id.ToString();
      cmd.Parameters.Add(patientIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundPatientId = 0;
      string foundPatientName = null;
      string foundPatientBirthday = null;
      int foundPatientDoctorId = 0;

      while(rdr.Read())
      {
        foundPatientId = rdr.GetInt32(0);
        foundPatientName = rdr.GetString(1);
        foundPatientBirthday = rdr.GetString(3);
        foundPatientDoctorId = rdr.GetInt32(2);
      }
      Patient foundPatient = new Patient(foundPatientName, foundPatientBirthday,  foundPatientDoctorId, foundPatientId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundPatient;
    }


    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patients;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }

}
