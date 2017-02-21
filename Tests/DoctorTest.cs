using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Hospital.Objects;

namespace Hospital
{
  public class DoctorTest : IDisposable
  {
    public DoctorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Hospital_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DoctorEmptyAtFirst()
    {
      //Arrange, Act
      int result = Doctor.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Doctor firstDoctor = new Doctor("Dr. Shruti", "Cardiology", 2);
      Doctor secondDoctor = new Doctor("Dr. Shruti", "Cardiology", 2);

      //Assert
      Assert.Equal(firstDoctor, secondDoctor);
    }


      [Fact]
    public void Test_Save_SavesDoctorToDatabase()
    {
      //Arrange
      Doctor testDoctor = new Doctor("Dr. Nick", "Neurologist",1);
      testDoctor.Save();

      //Act
      List<Doctor> result = Doctor.GetAll();
      List<Doctor> testList = new List<Doctor>{testDoctor};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsDoctorInDatabase()
    {
      //Arrange
      Doctor testDoctor = new Doctor("Dr. John", "Debugging");
      testDoctor.Save();

      //Act
      Doctor foundDoctor = Doctor.Find(testDoctor.GetId());

      //Assert
      Assert.Equal(testDoctor, foundDoctor);
    }

    [Fact]
    public void Test_GetDoctor_RetrievesAllDoctorsWithSpecialty()
    {
      Doctor firstDoctor = new Doctor("Dr. John", "Cardiology");
      Doctor secondDoctor = new Doctor("Dr. Shruti", "Cardiology");

      firstDoctor.Save();
      secondDoctor.Save();

      List<Doctor> doctorList = Doctor.GetDoctor("Cardiology");
      List<Doctor> testList = new List<Doctor> {firstDoctor, secondDoctor};

      Assert.Equal(doctorList, testList);
    }

    // [Fact]
    //  public void Test_GetDoctors_RetrievesAllDoctorsWithSpecialty()
    //  {
    //    Doctor testDoctor = new Doctor("Household chores");
    //    testDoctor.Save();
    //
    //    Patient firstPatient = new Patient("Mow the lawn", testDoctor.GetId());
    //    firstPatient.Save();
    //    Patient secondPatient = new Patient("Do the dishes", testDoctor.GetId());
    //    secondPatient.Save();
    //
    //
    //    List<Patient> testPatientList = new List<Patient> {firstPatient, secondPatient};
    //    List<Patient> resultPatientList = testDoctor.GetPatients();
    //
    //    Assert.Equal(testPatientList, resultPatientList);
    //  }

    public void Dispose()
    {
      // Patient.DeleteAll();
      Doctor.DeleteAll();
    }
  }
}
