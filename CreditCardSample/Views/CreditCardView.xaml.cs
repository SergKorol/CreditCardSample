using System.Globalization;
using CreditCardApp.Helpers.ExtensionMethods;
using CreditCardSample.Helpers;

namespace CreditCardSample.Views;

public partial class CreditCardView
{
    public static readonly BindableProperty CardNumberProperty
        = BindableProperty.Create(nameof(CardNumber), 
            typeof(string), typeof(CreditCardView), 
            propertyChanged: OnCardNumberChanged);

    public string CardNumber
    {
        get => (string)GetValue(CardNumberProperty);
        set => SetValue(CardNumberProperty, value);
    }

    public static readonly BindableProperty ExpirationDateProperty
        = BindableProperty.Create(nameof(ExpirationDate), 
            typeof(DateTime), typeof(CreditCardView), DateTime.Now, 
            propertyChanged: OnExpirationDateChanged);

    public DateTime ExpirationDate
    {
        get => (DateTime)GetValue(ExpirationDateProperty);
        set => SetValue(ExpirationDateProperty, value);
    }

    public static readonly BindableProperty CardValidationCodeProperty
    = BindableProperty.Create(nameof(CardValidationCode), 
        typeof(string), typeof(CreditCardView), "-", 
        propertyChanged: OnCardValidationCodeChanged);

    public string CardValidationCode
    {
        get => (string)GetValue(CardValidationCodeProperty);
        set => SetValue(CardValidationCodeProperty, value);
    }
    
    public static readonly BindableProperty IsFlippedProperty =
        BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(CreditCardView), false);

    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set
        {
            SetValue(IsFlippedProperty, value);
            UpdateCardSideVisibility();
        }
    }

    
    
    public CreditCardView()
    {
        InitializeComponent();
        
        CardFront.BackgroundColor = "Default".ToColorFromResourceKey();
        CardBack.BackgroundColor = "Default".ToColorFromResourceKey();

        CreditCardImageLabel.Text = "\uf09d";
        CreditCardImageLabel.FontFamily = "FA6Regular";

        ExpirationDateLabel.Text = "-";

        CardValidationCodeLabel.Text = $"CVC: -";
        CardBack.IsVisible = false;
        CardFront.IsVisible = true;
        Streak.IsVisible = false;
        
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnCardTapped;
        this.GestureRecognizers.Add(tapGestureRecognizer);
        
    }

    private void SetCreditCardNumber()
    {
        if (string.IsNullOrEmpty(CardNumber))
        {
            CardFront.BackgroundColor = (Color)Application.Current?.Resources["Default"];
            CardBack.BackgroundColor = (Color)Application.Current?.Resources["Default"];
            CreditCardImageLabel.Text = "\uf09d";
            CreditCardImageLabel.FontFamily = "FA6Regular";
        }

        if (long.TryParse(CardNumber, out long cardNumberAsLong))
        {
            CreditCardNumber.Text = 
                string.Format("{0:0000  0000  0000  0000}", cardNumberAsLong);
        }
        else
        {
            CreditCardNumber.Text = "-";
        }

        if (CardNumber != null)
        {
            var normalizedCardNumber = CardNumber.Replace("-", string.Empty);

            if (CardValidationHelper.AmericanExpress.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "AmericanExpress".ToColorFromResourceKey();
                CardBack.BackgroundColor = "AmericanExpress".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf1f3";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else if (CardValidationHelper.DinersClub.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "DinersClub".ToColorFromResourceKey();
                CardBack.BackgroundColor = "DinersClub".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf24c";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else if (CardValidationHelper.Discover.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "Discover".ToColorFromResourceKey();
                CardBack.BackgroundColor = "Discover".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf1f2";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else if (CardValidationHelper.JCB.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "JCB".ToColorFromResourceKey();
                CardBack.BackgroundColor = "JCB".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf24b";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else if (CardValidationHelper.MasterCard.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "MasterCard".ToColorFromResourceKey();
                CardBack.BackgroundColor = "MasterCard".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf1f1";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else if (CardValidationHelper.Visa.IsMatch(normalizedCardNumber))
            {
                CardFront.BackgroundColor = "Visa".ToColorFromResourceKey();
                CardBack.BackgroundColor = "Visa".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf1f0";
                CreditCardImageLabel.FontFamily = "FA6Brands";
            }
            else
            {
                CardFront.BackgroundColor = "Default".ToColorFromResourceKey();
                CardBack.BackgroundColor = "Default".ToColorFromResourceKey();
                CreditCardImageLabel.Text = "\uf09d";
                CreditCardImageLabel.FontFamily = "FA6Regular";
            }
        }
    }

    private void SetExpirationDate()
    {
        ExpirationDateLabel.Text = ExpirationDate.ToString("MM/yy", 
            CultureInfo.InvariantCulture);
    }

    private void SetCardValidationCode()
    {
        CardValidationCodeLabel.Text = $"CVC: {CardValidationCode}";
    }
    
    private static void OnCardNumberChanged(BindableObject bindable, 
        object oldValue, object newValue)
    {
        if (bindable is not CreditCardView creditCardView)
            return;

        creditCardView.SetCreditCardNumber();
    }

    private static void OnExpirationDateChanged(BindableObject bindable, 
        object oldValue, object newValue)
    {
        if (bindable is not CreditCardView creditCardView)
            return;

        creditCardView.SetExpirationDate();
    }

    private static void OnCardValidationCodeChanged(BindableObject bindable, 
        object oldValue, object newValue)
    {
        if (bindable is not CreditCardView creditCardView)
            return;

        creditCardView.SetCardValidationCode();
    }
    
    private void OnCardTapped(object sender, EventArgs e)
    {
        IsFlipped = !IsFlipped;
    }

    private void UpdateCardSideVisibility()
    {
        if (IsFlipped)
        {
            CardFront.IsVisible = false;
            CardBack.IsVisible = true;
            Streak.IsVisible = true;
        }
        else
        {
            CardFront.IsVisible = true;
            CardBack.IsVisible = false;
            Streak.IsVisible = false;
        }
    }
}