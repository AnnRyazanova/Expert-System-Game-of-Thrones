(defrule dwarf
	(gender male)
	=>
	(printout t "Is this character a dwarf? (yes / no)" crlf)
	(assert (dwarf (read))))

(defrule height
	(dwarf yes)
	=>
	(assert (height low)))

(defrule height
	(dwarf no)
	=>
	(printout t "Is this character tall? (yes / no)" crlf)
	(assert (is-tall (read))))

	(defrule is-tall
		(is-tall yes)
		=>
		(assert (height high)))

	(defrule is-tall
		(is-tall no)
		=>
		(assert (height low)))

(defrule warrior
	=>
	(printout t "Is this character a warrior? (yes / no)" crlf)
	(assert (warrior (read))))

(defrule force
	(warrior yes)
	=>
	(assert (force strong)))

(defrule force
	(warrior no)
	=>
	(assert (force weak)))

(defrule is-wolf
	=>
	(printout t "Is there a wolf on the embleme? (yes / no)" crlf)
	(assert (is-wolf (read))))

(defrule family
	(is-wolf yes)
	=>
	(assert (family stark)))

(defrule is-lion
	(is-wolf no)
	=>
	(printout t "Is there a lion on the embleme? (yes / no)" crlf)
	(assert (is-lion (read))))

(defrule is-deer
	(is-lion no)
	=>
	(printout t "Is there a deer on the embleme? (yes / no)" crlf)
	(assert (is-deer (read))))

(defrule is-dragon
	(is-deer no)
	=>
	(printout t "Is there a dragon on the embleme? (yes / no)" crlf)
	(assert (is-dragon (read))))

(defrule commander 
	(gender male)
	(warrior yes)
	(is-wolf yes)
	=>
	(printout t "Was he the commander of the night watch? (yes / no)" crlf)
	(assert (commander (read))))	

(defrule gender
	=>
	(printout t "What is the gender of the character? (male / female)" crlf)
	(assert (gender (read))))
	
(defrule family
	(is-lion yes)
	=>
	(assert (family lannister)))

(defrule family
	(is-deer yes)
	=>
	(assert (family baratheon)))

(defrule family
	(is-dragon yes)
	=>
	(assert (family targaryen)))

(defrule executed
	(gender male)
	(is-wolf yes)
	=>
	(printout t "Was this character executed? (yes / no)" crlf)
	(assert (executed (read))))

(defrule lost-hand
	(gender male)
	(is-lion yes)
	(warrior yes)
	=>
	(printout t "Did this character lose hand? (yes / no)" crlf)
	(assert (lost-hand (read))))
	
(defrule faceless
	(gender female)
	(is-wolf yes)
	(warrior yes)
	=>
	(printout t "Is this character assosiated with Order of Faceless? (yes / no)" crlf)
	(assert (faceless (read))))
	
(defrule has-dragons
	(gender female)
	(is-dragon yes)
	=>
	(printout t "Does this character have dragons? (yes / no)" crlf)
	(assert (has-dragons (read))))
	
(defrule was-king
	(gender male)
	(is-deer yes)
	=>
	(printout t "Was the character king of Seven Kingdoms? (yes / no)" crlf)
	(assert (was-king (read))))

(defrule is-queen
	(gender female)
	(is-lion yes)
	=>
	(printout t "Is this character queen? (yes / no)" crlf)
	(assert (is-queen (read))))
	
(defrule tirion
	(gender male)
	(dwarf yes)
	(force weak)
	(is-lion yes)
	=>
	(printout t "This is Tirion Lannister" crlf))

(defrule ned
	(gender male)
	(warrior yes)
	(executed yes)
	(is-wolf yes)
	(is-tall yes)
	=>
	(printout t "This is Ned Stark" crlf))

(defrule jeime
	(gender male)
	(warrior yes)
	(is-lion yes)
	(is-tall yes)
	(lost-hand yes)
	=>
	(printout t "This is Jeime Lannister" crlf))

(defrule jon
	(gender male)
	(warrior yes)
	(is-tall yes)
	(commander yes)
	=>
	(printout t "This is Jon Snow" crlf))

(defrule arya
	(gender female)
	(is-wolf yes)
	(warrior yes)
	(faceless yes)
	=>
	(printout t "This is Arya Stark" crlf))
	
(defrule daenerys
	(gender female)
	(is-dragon yes)
	(has-dragons yes)
	(warrior no)
	=>
	(printout t "This is Daenerys Targaryen" crlf))
	
(defrule robert
	(gender male)
	(warrior yes)
	(is-deer yes)
	(was-king yes) 
	=>
	(printout t "This is Robert Baratheon" crlf))
	
(defrule cersei
	(gender female)
	(is-lion)
	(is-queen yes)
	=>
	(printout t "This is Cersei Lannister" crlf))