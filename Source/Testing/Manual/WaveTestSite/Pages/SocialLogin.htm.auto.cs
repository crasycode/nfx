//WARNING: This code was auto generated by template compiler, do not modify by hand!
//Generated on 19.10.2015 22:37:06 by NFX.Templatization.TextCSTemplateCompiler at M6-1271

using System; 
using System.Text; 
using System.Linq; 
using System.Collections.Generic; 
using NFX.Wave.Templatization; 
using NFX.Web; 

namespace WaveTestSite.Pages 
{

 ///<summary>
 /// Social Login
 ///</summary>
 public  class SocialLogin : NFX.Wave.Templatization.WaveTemplate
 {

     protected override void DoRender()
     {
       base.DoRender();
        Target.Write( SocialLogin._90_S_LITERAL_0 );
        Target.Write(Target.Encode( DateTime.Now ));
        Target.Write( SocialLogin._90_S_LITERAL_1 );
      foreach(var provider in WebSettings.SocialNetworks) {
        Target.Write( SocialLogin._90_S_LITERAL_2 );
        Target.Write(Target.Encode( provider.Name ));
        Target.Write( SocialLogin._90_S_LITERAL_3 );
      }
        Target.Write( SocialLogin._90_S_LITERAL_4 );

     }


     #region Literal blocks content
        private const string _90_S_LITERAL_0 = @"

<!DOCTYPE html>
<html>
  <head>
    <title>Social Login</title>
    <style type=""text/css"">
      body {
        background-color: black;
        color: white;
      }

      a {
        text-decoration: none;
        color: #43aed0;
      }

      .loginWrapper {
        width: 600px;
        margin-left: auto;
        margin-right: auto;
        text-align: center;
      }

      .socialWrapper {
        background-color: #505050;
      }
    </style>
  </head>
  <body>
    <div class=""loginWrapper"">
      <h1>Login As</h1>
      "; 
        private const string _90_S_LITERAL_1 = @"
      "; 
        private const string _90_S_LITERAL_2 = @"
        <div class=""socialWrapper"">
          <p>"; 
        private const string _90_S_LITERAL_3 = @"</p>
          <p><a href=""#"">Obtain New Token</a></p>
        </div>
      "; 
        private const string _90_S_LITERAL_4 = @"
      
  </body>
</html>"; 
     #endregion

 }//class
}//namespace
