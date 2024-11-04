using Candidate_BusinessObject;
using Candidate_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidateManagement_LeTienDat
{
    public partial class LoginWindow : Window
    {
        private IHRAccountService iHRAccountService;

        public LoginWindow()
        {
            InitializeComponent();
            iHRAccountService = new HRAccountService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Hraccount hrAccount = iHRAccountService.GetHraccountByEmail(txtEmail.Text);
            if (hrAccount != null && txtPassword.Password.Equals(hrAccount.Password))
            {
                int? roleID = hrAccount.MemberRole;
                switch (roleID)
                {
                    case 1:
                        this.Hide();
                        CandidateProfileManager candForm = new CandidateProfileManager(roleID);
                        candForm.Show();
                        break;
                    case 2:
                        this.Hide();
                        CandidateProfileManager staffCandidate = new CandidateProfileManager(roleID);
                        staffCandidate.Show();
                        break;
                    case 3:
                        this.Hide();
                        CandidateProfileManager managementCandidate = new CandidateProfileManager(roleID);
                        managementCandidate.Show();
                        break;
                    default:
                        break;
                }

            }
            else
            {
                MessageBox.Show("Wrong email or password");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
