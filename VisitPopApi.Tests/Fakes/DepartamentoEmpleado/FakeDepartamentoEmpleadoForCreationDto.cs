﻿using AutoBogus;
using VisitPop.Application.Dtos.DepartamentoEmpleado;

namespace VisitPopApi.Tests.Fakes.DepartamentoEmpleado
{
    // or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
    public class FakeDepartamentoEmpleadoForCreationDto : AutoFaker<DepartamentoEmpleadoForCreationDto>
    {
        public FakeDepartamentoEmpleadoForCreationDto()
        {
            // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
            //RuleFor(a => a.ExampleIntProperty, a => a.Random.Number(50, 100000));
            //RuleFor(a => a.ExampleDateProperty, a => a.Date.Past()); 
        }
    }
}
