using UI;

ApplicationConfiguration.Initialize();

// יצירת טופס הכניסה
using (LoginForm login = new LoginForm())
{
    // אם המשתמש בחר תפקיד ולחץ על כפתור
    if (login.ShowDialog() == DialogResult.OK)
    {
        // הרצת החלון הראשי עם הפרמטר של האם הוא מנהל
        Application.Run(new MainWindow(login.IsAdmin));
    }
}