namespace TimeSheetApp.Model.EntitiesBase
{
    public class Analytic
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public int DepartmentId { get; set; }
        public int DirectionId { get; set; }
        public int UpravlenieId { get; set; }
        public int OtdelId { get; set; }
        public int PositionsId { get; set; }
        public int RoleTableId { get; set; }
        public int? HeadAdmId { get; set; }
        public int? HeadFuncId { get; set; }
        public bool? Deleted_Flag { get; set; }
        public virtual Departments Departments { get; set; }
        public virtual Directions Directions { get; set; }
        public virtual Upravlenie Upravlenie { get; set; }
        public virtual Otdel Otdel { get; set; }
        public virtual Positions Positions{get;set;}
        public virtual Role Role { get; set; }
        public virtual Analytic AdminHead { get; set; }
        public virtual Analytic FunctionHead { get; set; }
    }
}
