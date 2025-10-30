using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Constants.Text
{
    public class DkString
    {
        public const String Yes = "نعم";
        public const String No = "لا";
        public const String LoginTitle = "تسجيل الدخول";
        public const String Email = "البريد الاكتروني";
        public const String Password = "كلمة السر";
        public const String ConfirmPassword = "تاكيد كلمة السر";
        public const String RememberMe = "تذكرني؟";
        public const String DoYouForgetPassword = "هل نسيت كلمة السر ؟";
        public const String ResetPassword = "استرجاع كلمة السر";
        public const String ResetPasswordRequest = "طلب استرجاع كلمة السر";
        public const String ResetPasswordConfirm = "تغيير كلمة السر";
        public const String SendOtp = "ارسال كود التفعيل";
        public const String Otp = "كود التفعيل";
        public const String Back = "عودة";
        public const String RegisterTitle = "تسجيل مستخدم جديد";
        public const String UserName = "أسم تسجيل الدخول";
        public const String UserFullName = "أسم المستخدم الثلاثي";
        public const String JobTitle = "المهنة";
        public const String PhoneNumber = "رقم الهاتف";
        public const String MemberQuestion = "هل انت عضو سابق في النقابة؟";
        public const String IdNumber = "رقم الهوية النقابية";
        public const String IdPhoneNumber = "رقم الهاتف في استمارة الانتماء";
        public const String ConfirmEmail = "تفعيل البريد الاكتروني";
        public const String ReSendConfirmOtp = "اعادة ارسال رمز التفعيل";
        public const String EmailSubject = "تفعيل الاشتراك";

        // Comments 
        public const String RegisterComment01 = "كلمة السر يجب ان تكون باللغة الانكليزية ولاتقل عن 8 احرف مع حرف واحد كبير على الاقل";
        public const String RegisterComment02 = "اسم تسجيل الدخول يجب ان يكون باللغة الانكليزية ولايقل عن 6 احرف";
        public const String ResetPasswordComment = "ادخل البريد الاكتروني الذي قمت بالتسجيل من خلاله وسوف نرسل لك كود التفعيل رجاءا";
        public const String ResetPasswordComment01 = "اكتب كلمة السر الجديدة وتاكيد كلمة السر مع رمز التفعيل الذي تم ارساله لك عير البريد الاكتروني رجاءا";
        public const String ConfirmEmailComment = "اكتب رمز التفعيل الذي تم ارساله الى بريدكم الاكتروني لتفعيل الاشتراك رجاءا";
        public const String ReSendConfirmOtpComment = "يرجى كتابة البريد الاكتروني الذي قمت بالتسجيل من خلاله وسوف نرسل لك كود التفعيل رجاءا";
        // Error Messages 
        public const String EmailError01 = "يجب كتابة البريد الاكتروني ";
        public const String EmailError02 = "يجب كتابة البريد الاكتروني بالطريقة الصحيحة";
        public const String PasswordError01 = "يجب كتابة كلمة السر رجاءا";
        public const String PasswordError02 = "كلمة السر يجب ان تتكون من 8 احرف على الاقل رجاءا";
        public const String PasswordError03 = "يجب تاكيد كلمة السر رجاءا";
        public const String PasswordError04 = "كلمة السر غير متطابقة رجاءا";
        public const String UserFullNameError01 = "يجب كتابة الاسم الثلاثي رجاءا";
        public const String JobTitleError01 = "يجب كتابة المهنة رجاءا";
        public const String PhoneNumberError01 = "يجب كتابة رقم الهاتف رجاءا";
        public const String OtpError01 = "يجب كتابة كود التفعيل رجاءا";
        public const String GoogleRegisterError01 = "استخدم حساب كوكل للتسجيل الدخول رجاءا";
        public const String UserExistError = "انت مشترك سابقا يرجى تسجيل الدخول";
        public const String InvalidRequestError = "حدث خطأ ما يرجى المحاولة مرة اخرى";
    }
}