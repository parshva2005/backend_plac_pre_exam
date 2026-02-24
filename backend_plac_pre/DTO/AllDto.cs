namespace backend_plac_pre.DTO
{

    #region User_Dto
    public class User_Dto
    {
        public string? Name { get; set; }
    }
    #endregion

    #region User_Login_Dto
    public class User_Login_Dto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    #endregion

    #region Register_Dto
    public class Register_Dto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
    }
    #endregion

    #region User Profile DTO
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    #endregion

    #region Auth_Response_Dto
    public class Auth_Response_Dto
    {
        public string Token { get; set; }
        public UserProfileDto User { get; set; }
    }
    #endregion

    #region Ticket_Dto
    public class Ticket_Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssignedToUserId { get; set; }
    }
    #endregion

    #region Ticket_Update_Dto
    public class Ticket_Update_Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssignedToUserId { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }
    #endregion

    #region Comment_Dto
    public class Comment_Dto
    {
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public int CreedByUserId { get; set; }
    }
    #endregion

    #region Ticket_Status_Log_Dto
    public class Ticket_Status_Log_Dto
    {
        public int TicketId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public int ChangedByUserId { get; set; }
    }
    #endregion

    #region Commnet_Perameter_Dto
    public class Comment_Perameter_Dto
    {
        public string Comment { get; set; }
    }
    #endregion

}
