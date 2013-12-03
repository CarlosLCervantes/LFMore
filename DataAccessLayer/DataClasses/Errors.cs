using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class Errors
{
    public static string USERNAME_TAKEN = "The User Name you selected is already taken";
    public static string CHECK_USER_NAME = "A User Name must be between 5 and 20 characters";
    public static string DATE_INVALID = "The date entered is invalid";
    public static string TIME_INVALID = "The time entered was not valid";
    public static string CHARACTER_NOT_FOUND = "Couldn't find the character requested";
    public static string MEMBER_ALREADY_HAS_CHARACTER_IN_EVENT = "There is another character in this event which belongs to same member.";
    public static string CAN_NOT_OPERATE_ON_OTHER_MEMBERS_DATA = "Member Information Was Invalid";
    public static string CAN_NOT_OPERATE_ON_OTHER_MEMBERS_CHARACTER = "Character Information Was Invalid";
    public static string EDIT_CHARACTER_FAILED = "Failed to Edit Character Info";
    public static string NO_ROLES_SELECTED = "No Roles Were Selected";
    public static string ROLE_IS_NOT_VALID_FOR_CLASS = "This class can't perform one or more of the selected roles";
    public static string INSERT_CHARACTER_FAILED = "Failed to create new Character";
    public static string INVALID_CHARACTER_INFORMATION = "Invalid Character Information";
    public static string INVALID_EVENT_ID = "Could not Modify Event";
    public static string NOT_EVENT_CREATOR = "You are not authorized to Edit Event information";
    public static string SEARCH_TERM_CANNOT_BE_EMPTY = "Search Term Cant be empty";
    public static string MEMBER_ALREADY_ADDED_THIS_GAME = "You have already added this game";
    public static string ATTENDEE_COULDNT_BE_FOUND = "Attendee does not exist";
    public static string MUST_AGREE_TO_TERMS = "You must agree to the Terms";
    public static string INVALID_EMAIL = "Invalid Email";
    public static string CHECK_PASSWORD = "A Password must be between 6 and 14 characters";
}

