using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Hospital.Objects;

namespace Hospital
{
  public class PatientTest : IDisposable
  {
    public PatientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Hospital_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Patient.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Patient.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueIfDescriptionAreTheSame()
    {
      Patient firstPatient = new Patient("nick", "June 25 1993", 2,1);
      Patient secondPatient = new Patient("shruti", "October 8 1988", 3,1);

      Assert.Equal(firstPatient, secondPatient);
    }

    [Fact]
    public void  Test_Save_SavesToDatabase()
    {
     //Arrange
      Patient testPatient = new Patient ("John", "April 1 1945", 5);

      //Act
      testPatient.Save();
      List<Patient> result = Patient.GetAll();
      List<Patient> testList = new List<Patient>{testPatient};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Patient testPatient = new Patient ("John", "April 1 1954", 5);
      //Act
      testPatient.Save();
      Patient savedPatient = Patient.GetAll()[0];

      int result = savedPatient.GetId();
      int testId = testPatient.GetId();

      //Assert
      Assert.Equal(testId,result);
    }

    [Fact]
    public void Test_Find_FindsPatientInDatabase()
    {
      Patient testPatient = new Patient("Minh", "June 30 1993", 5);
      testPatient.Save();

      Patient foundPatient = Patient.Find(testPatient.GetId());

      Assert.Equal(testPatient, foundPatient);
    }
  }
}
