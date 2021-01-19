using System;
using System.Collections.Generic;
using System.Text;

namespace VisitPop.Domain.Constants
{
    /// <summary>
    /// Constantes para definir tamaños de las propiedades en las entidades
    /// </summary>
    public class VisitEntityConstants
    {
        public const int MAX_NAMES_LENGTH = 100;
        public const int MAX_DESCRIPTION_LENGTH = 250;
        public const int MAX_NOTES_LENGTH = 250;

        public const int MAX_PHONE_LENGTH = 13;
        public const int MAX_DOC_ID_LENGTH = 13;

        public const int MAX_EMAIL_LENGTH = 254;

        public const int MINIMUM_QUANTITY = 0;
    }
}
