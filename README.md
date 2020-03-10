# Hamcat (Amateur Radio CAT) library for .NET

## A C# library for Amateur Radio CAT

Compatible with Kenwood, Yeasu devices and Hamlib library

## Usage
- initialize CatKenwood or CatYaesu or CatHamlib class
- call Open() method
- call an available command
- call Close() method

## Available commands
### public void TX()
Set the transceiver in TX mode

### public void RX()
Set the transceiver in RX mode

### public void LowPower()
Set the low power

### public void DisableATU()
Disable antenna tuner

### public void SetFrequency(int freq)
Set VFO frequency

### public void SetMode(Modes mode)
Set operating mode (LSB, USB, CW, FM, AM, DIG)

### public void AskSwr()
Get SWR 

### public void AskFrequency()
Get VFO frequency

## Properties
- Modes Mode {get; set;} - operating mode
- double Swr {get;} - SWR
- int Comp {get;} - COMP
- int Alc {get;} - ALC
- int Freq {get; set;} - VFO frequency
- bool Tx {get; set;} - TX/RX

