using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Hospital.Objects;

namespace Hospital
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Doctor> AllDoctors = Doctor.GetAll();
        return View["index.cshtml", AllDoctors];
      };
      Get["/patients"] = _ => {
            List<Patient> AllPatients = Patient.GetAll();
            return View["patients.cshtml", AllPatients];
      };
      Get["/doctors"] = _ => {
        List<Doctors AllDoctors  = Doctors GetAll();
        return View["doctors.cshtml", AllDoctors ];
      };
    }
  }
}
