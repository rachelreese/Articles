﻿module FormatTextViewModel 

    open System.ComponentModel
    open System.Threading
    open System.Windows.Documents
    open System.Windows.Media
    open System.Windows

    type FormatTextViewModel() =
        let mutable text = ""
        let mutable isWvowel = false
        let mutable isYvowel = false
        let mutable formattedText = new FlowDocument()

        // lots of things omitted
        
        let propChanged = Event<PropertyChangedEventHandler,PropertyChangedEventArgs>()
        
        let ConvertCharacter unformattedText = 
            let character, isv = unformattedText
            let fg = 
                if isv then
                    Brushes.OrangeRed
                else
                    Brushes.DarkSlateGray
            let run = new Run(character.ToString(), Foreground = fg)
            run

        let BuildDocument (unformattedText:(char*bool)[]) =
            let fd = new FlowDocument()
            let par = new Paragraph()
            unformattedText
                |> Array.map ConvertCharacter
                |> par.Inlines.AddRange
            fd.Blocks.Add(par)
            fd

        member this.GetFormattedText = 
            BuildDocument <| FormatTextModel.CheckForVowels this.IsWVowel this.IsYVowel (this.Text.ToString().ToCharArray())

        member this.Text 
            with get () = text
            and set value =
                text <- value
                this.OnPropertyChanged("Text")

        member this.IsWVowel
            with get() = isWvowel
            and set value = 
                isWvowel <- value 
                this.OnPropertyChanged("IsWVowel")

        member this.IsYVowel
            with get() = isYvowel
            and set value = 
                isYvowel <- value 
                this.OnPropertyChanged("IsYVowel")

        member this.OnPropertyChanged(name) =
            propChanged.Trigger(this, new PropertyChangedEventArgs(name))

        [<CLIEvent>]
        member this.PropertyChanged = propChanged.Publish

        interface INotifyPropertyChanged with
            [<CLIEvent>]
            member this.PropertyChanged = this.PropertyChanged   
