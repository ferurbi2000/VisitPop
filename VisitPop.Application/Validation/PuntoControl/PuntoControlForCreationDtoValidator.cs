using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PuntoControl;

namespace VisitPop.Application.Validation.PuntoControl
{
    public class PuntoControlForCreationDtoValidator:PuntoControlForManipulationDtoValidator<PuntoControlForCreationDto>
    {
        // add fluent validation rules that should be shared between creation and update operations here
        //https://fluentvalidation.net/
    }
}
