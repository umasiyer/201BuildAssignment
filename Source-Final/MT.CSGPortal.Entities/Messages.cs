using System.Collections.Generic;

namespace MT.CSGPortal.Entities
{
    public class Messages
    {
        private IList<string> successMessages;
        private IList<string> errorMessages;
        private IList<string> warningMessages;

        public Messages()
        {
            successMessages = new List<string>();
            errorMessages = new List<string>();
            warningMessages = new List<string>();
        }

        public IList<string> SuccessMessages { get {return successMessages;} set { successMessages=value;} }
        public IList<string> ErrorMessages { get { return errorMessages; } set { errorMessages = value; } }
        public IList<string> WarningMessages { get { return warningMessages; } set { warningMessages = value; } }
    }
}
