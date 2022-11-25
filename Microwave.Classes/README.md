--Class Diagram-- 

@startuml
hide members

class Display
class Output
class CookController
class PowerTube
class Timer
class Light
class UserInterface
class Door
class Button 
class Beep

class IDisplay
class IOutput
class ICookController
class IPowerTube
class ITimer
class ILight
class IUserInterface
class IDoor
class IButton 
class IBeep

CookController <-d-> Timer
CookController -l-> PowerTube
CookController -u-> Display
CookController <-r-> UserInterface
CookController -l-> Beep
  UserInterface -l-> Display
  UserInterface -u-> Light

 
 UserInterface .d.> Button
 UserInterface .d.> Door
 Door -r-> UserInterface
 Button "3"-d-> UserInterface 

Output <-r- Light
Output <-r- Display
Output <-d- PowerTube
Beep -l-> Output


@enduml

-- Sequence Diagram --

@startuml
!theme plain

actor "User" as user
participant ":Door" as door
participant "PowerButton:\nButton " as powerbutton 
participant "TimerButton:\nButton" as timerbutton
participant "Start-CancelButton:\n Button " as startbutton
participant ": UserInterface" as ui 
participant ": Display" as display
participant ": Light" as light
participant ": CookController" as cookcontroller
participant ": Timer" as timer
participant ": PowerTube" as powertube 
participant ": Beep" as beep
participant ": Output" as output 


hnote over ui : Ready
user ->> door : open 
activate door 
door ->> ui : <<event>> OnDoorOpened()
deactivate door 
activate ui 
ui ->> light : TurnOn()
deactivate ui 
activate light
light ->> output : LogLine()
deactivate light

hnote over ui : DoorIsOpen
user ->> door : Closes Door
activate door
door ->> ui : <<event>> OnDoorClosed()
deactivate door
activate ui
ui ->> light : TurnOff()
deactivate ui
activate light
light ->> output : LogLine()
deactivate light

hnote over ui : Ready
loop until power set
user ->> powerbutton : Press PowerButton
activate powerbutton
powerbutton ->> ui : <<event>> OnPowerPressed()
deactivate powerbutton
activate ui
ui ->> display : ShowPower()
deactivate ui
activate display
display ->> output : LogLine()
deactivate display
hnote over ui : SetPower
end



loop until time set
user ->> timerbutton : Press TimerButton
activate timerbutton
timerbutton ->> ui : <<event>> OnTimePressed()
deactivate timerbutton
activate ui
ui ->> display : ShowTime()
deactivate ui
activate display
display ->> output : LogLine()
deactivate display
hnote over ui : SetTime
end


user ->> startbutton : Press Start-CancelButton 
activate startbutton
startbutton ->> ui : <<event>> OnStartCancelPressed()
deactivate startbutton
activate ui
ui ->> light : TurnOn()
activate light
light ->> output : LogLine()
deactivate light
ui ->> cookcontroller : StartCooking()
deactivate ui
activate cookcontroller
cookcontroller ->> timer : Start()
cookcontroller ->> powertube : TurnOn()
deactivate cookcontroller
activate powertube
powertube ->> output : LogLine()
deactivate powertube


hnote over ui : Cooking

loop until time added

loop until time expired

timer -> cookcontroller : <<event>> OnTimerTick()
activate cookcontroller

user ->> timerbutton : Press TimerButton
activate timerbutton
timerbutton->> ui : <<event>> OnTimePressed()
deactivate timerbutton
activate ui
ui ->> cookcontroller : ChangeTimeWhileCooking()
deactivate ui




cookcontroller ->> timer : get TimeRemaining
activate timer
timer --> cookcontroller : 
cookcontroller ->> timer : Start()
deactivate timer
cookcontroller ->> display : ShowTime()
deactivate cookcontroller
activate display
display ->> output : LogLine()
deactivate display
end

timer -> cookcontroller : <<event>> OnTimerExpired()
activate cookcontroller
hnote over ui : AddTime
end

activate cookcontroller
cookcontroller ->> powertube : TurnOff()
activate powertube
powertube ->> output : LogLine() 
deactivate powertube
cookcontroller ->> beep : PlayBeep()
activate beep
beep -> output : Beep sound
deactivate beep
cookcontroller ->> ui : CookingIsDone()
deactivate cookcontroller
activate ui
ui ->> display : Clear()
activate display
display ->> output : LogLine()
deactivate display
ui ->> light : TurnOff()
deactivate ui
activate light 
light ->> output : LogLine()
deactivate light


hnote over ui : Ready
@enduml
  
  
  -- Beep -- 
  
  @startuml
!theme plain

participant ": CookController" as cookcontroller
participant ": Timer" as timer

participant ": Beep" as beep
participant ": Output" as output 



timer -> cookcontroller : <<event>> OnTimerExpired()
activate cookcontroller
cookcontroller ->> beep : PlayBeep()
deactivate cookcontroller
activate beep
beep -> output : Beep sound
deactivate beep


@enduml
  
 
 -- ChangeTimeWhileCooking -- 
  
  @startuml
!theme plain

actor "User" as user


participant "TimerButton:\nButton" as timerbutton

participant ": UserInterface" as ui 
participant ": Display" as display

participant ": CookController" as cookcontroller
participant ": Timer" as timer

participant ": Output" as output 

hnote over ui : Cooking

loop until time added

loop until time expired

timer -> cookcontroller : <<event>> OnTimerTick()
activate cookcontroller

user ->> timerbutton : Press TimerButton
activate timerbutton
timerbutton->> ui : <<event>> OnTimePressed()
deactivate timerbutton
activate ui
ui ->> cookcontroller : ChangeTimeWhileCooking()
deactivate ui




cookcontroller ->> timer : get TimeRemaining
activate timer
timer --> cookcontroller : 
deactivate timer
cookcontroller ->> timer : Start()
cookcontroller ->> display : ShowTime()
deactivate cookcontroller
activate display
display ->> output : LogLine()
deactivate display
end

hnote over ui : AddTime
end

@enduml

  
  -- State Machine -- 
  
  @startuml

'Ready
[*] -> Ready
Ready -> SetPower :Press Power Button/\nDisplay Power
Ready --> DoorOpen: DoorOpens/\nTurn On Light

'SetPower'
SetPower --> SetTime: TimeButtonPressed/\nDisplay Time

SetPower -> Ready :Start-Cancel ButtonPressed/\nReset Values,\nClear Display

SetPower -> SetPower: Press Power Button/\nIncrease Power,\nDisplay Power

SetPower -> DoorOpen: DoorIsOpened/\nReset Values,\nClear Display,\nTurn On Light


'SetTime
SetTime -down-> Cooking: Start-Cancel Button Pressed/\nStart Cooking,\nTurn On Light

SetTime -> SetTime: TimeButtonPressed/\nIncrease Time, \nDisplay Time

SetTime -> DoorOpen: DoorIsOpened/\nReset Values,\nClear Display,\nTurn On Light


'Cooking
Cooking -> Ready: Cooking Finished/\nReset Values,\nClear Display,\nTurnOffLight

Cooking -> Ready: Start-Cancel Button Pressed/\nStop Cooking,\nReset Values,\nClear Display,\nTurnOffLight

Cooking --> DoorOpen: DoorIsOpened/\nStop Cooking,\nPlayBeep\nReset Values,\nClear Display

Cooking -> Cooking: TimeButtonPressed/\nAdd To Remaing Time


'DoorOpen
DoorOpen -> Ready: DoorIsClosed/\nTurn Off Light
@enduml
