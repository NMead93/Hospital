using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Hospital.Objects
{
  public class Doctor
  {
    private string _name;
    private string _specialty;
    private int _id;

    public Doctor (string name, string specialty, int Id=0)
    {
      _name = name;
      _specialty= specialty;
      _id = Id;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string Name)
    {
      _name = Name;
    }
    public string GetSpecialty()
    {
      return _specialty;
    }

    public static List<Doctor> GetDoctor(string specialty)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * From doctors WHERE specialty = @DoctorSpecialty;", conn);
      SqlParameter DoctorSpecialtyParameter = new SqlParameter();
      DoctorSpecialtyParameter.ParameterName = "@DoctorSpecialty";
      DoctorSpecialtyParameter.Value = specialty;
      cmd.Parameters.Add(DoctorSpecialtyParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Doctor> doctorList = new List<Doctor>{};

      while(rdr.Read())
      {
        int doctorId = rdr.GetInt32(0);
        string doctorName = rdr.GetString(1);
        string doctorSpecialty = rdr.GetString(2);

        Doctor newDoctor = new Doctor(doctorName,doctorSpecialty, doctorId);
        doctorList.Add(newDoctor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return doctorList;
    }

  public override bool Equals(System.Object otherDoctor)
  {

    if (!(otherDoctor is Doctor)){
      return false;
    }
    else
    {
      Doctor newDoctor = (Doctor) otherDoctor;
      bool idEquality = this.GetId() == newDoctor.GetId();
      bool nameEquality = this.GetName() == newDoctor.GetName();
      bool specialtyEquality = this.GetSpecialty() == newDoctor.GetSpecialty();
      return (idEquality && specialtyEquality && nameEquality);
    }
  }

  public static List<Doctor> GetAll()
  {
    List<Doctor> allDoctor = new List<Doctor>{};

    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM doctors;", conn);
    SqlDataReader rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int DoctorId = rdr.GetInt32(0);
      string DoctorName = rdr.GetString(1);
      string DoctorSpecialty = rdr.GetString(2);
      Doctor newDoctor = new Doctor(DoctorName, DoctorSpecialty, DoctorId);
      allDoctor.Add(newDoctor);
    }

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }

    return allDoctor;
  }

  public void Save()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO doctors (name,specialty) OUTPUT INSERTED.id VALUES (@DoctorName,@DoctorSpecialty);", conn);

    SqlParameter nameParameter = new SqlParameter();
    SqlParameter specialtyParameter = new SqlParameter();
    nameParameter.ParameterName = "@DoctorName";
    specialtyParameter.ParameterName = "@DoctorSpecialty";
    nameParameter.Value = this.GetName();
    specialtyParameter.Value = this.GetSpecialty();
    cmd.Parameters.Add(nameParameter);
    cmd.Parameters.Add(specialtyParameter);
    SqlDataReader rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      this._id = rdr.GetInt32(0);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if(conn != null)
    {
      conn.Close();
    }
  }

  public static Doctor Find(int id)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM Doctors WHERE id = @DoctorId;", conn);
    SqlParameter DoctorIdParameter = new SqlParameter();
    DoctorIdParameter.ParameterName = "@DoctorId";
    DoctorIdParameter.Value = id.ToString();
    cmd.Parameters.Add(DoctorIdParameter);
    SqlDataReader rdr = cmd.ExecuteReader();

    int foundDoctorId = 0;
    string foundDoctorName = null;
    string foundSpecialty = null;

    while(rdr.Read())
    {
      foundDoctorId = rdr.GetInt32(0);
      foundDoctorName = rdr.GetString(1);
      foundSpecialty = rdr.GetString(2);
    }
    Doctor foundDoctor = new Doctor(foundDoctorName,foundSpecialty, foundDoctorId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundDoctor;
  }

  public static void DeleteAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM doctors;", conn);
    cmd.ExecuteNonQuery();
    conn.Close();
  }


}
}
