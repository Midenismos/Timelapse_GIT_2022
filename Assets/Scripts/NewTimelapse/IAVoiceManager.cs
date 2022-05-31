using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;


public class IAVoiceManager : MonoBehaviour
{
    public List<GameObject> Petales = new List<GameObject>();
    public string[] DialogueTexts;
    public AudioClip[] DialogueSounds;

    public Dialogue[] DialogueList;

    public Dialogue[] RandomAntiCasierDialogue;

    [SerializeField] int i = 0;

    private bool dialogueHappening = false;

    private AudioSource source;

    private TMP_Text txt = null;
    private bool _inCooldown = false;

    private void Awake()
    {
        DialogueList = new Dialogue[]
        {
        new Dialogue
        (   "Contexte",
            new string[]
            {
                "D�but de la mission INV0071-ND12-TAS.",
                "Nous sommes le 7 septembre 2246",
                "Bonjour enqu�trice 854-186-420.",
                "Je suis la nouvelle intelligence artificielle missionn�e pour les investigations." ,
                "Mon nom usuel est I A M I.",
                "Veuillez vous installer rapidement � votre poste pour commencer votre mission."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/1.Contexte/1") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/2") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/3") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/4") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/5") as AudioClip,
                Resources.Load("Sound/IA/1.Contexte/6") as AudioClip
            }
        ),
        new Dialogue
        (   "Presentation",
            new string[]
            {
                "Ma premi�re t�che consiste � vous pr�senter ou vous rappeler comment se servir avec efficacit� de l��quipement � votre disposition.",
                "Ma seconde t�che consiste � analyser votre efficacit� lors de votre mission.",
                "La commission pr�sid�e par le colonel Prescott pourra ensuite �tudier les r�sultats de votre enqu�te et d�terminer si vous avez rempli votre mission ou si vous avez �chou�.",
                "Pour commencer, lisez votre briefing de mission."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/2.Pr�sentation de l'IA/1") as AudioClip,
                Resources.Load("Sound/IA/2.Pr�sentation de l'IA/2") as AudioClip,
                Resources.Load("Sound/IA/2.Pr�sentation de l'IA/3") as AudioClip,
                Resources.Load("Sound/IA/2.Pr�sentation de l'IA/4") as AudioClip
            }
        ),
        new Dialogue
        (   "PresentationVideo",
            new string[]
            {
                "Comme vous avez pu le constater, le postulat initial de votre enqu�te observe un changement brutal du nombre de signes vitaux �mis par l'�quipage du Camilla Ship, le 2 septembre, entre 15h00 et 15h15.",
                "Avant d�aller plus loin dans la lecture d�autres documents, je vous invite � regarder les vid�os de surveillance du Camilla Ship.",
                "Malheureusement, il n�y a pas de sons sur ces vid�os : le r�sultat d�un manque d�entretien, typiquement humain.",
                "Vous allez devoir vous d�brouiller sans."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/2.Pr�sentation de l'IA/5") as AudioClip,
                Resources.Load("Sound/IA/3.Pr�sentation des videos/1") as AudioClip,
                Resources.Load("Sound/IA/3.Pr�sentation des videos/2") as AudioClip,
                Resources.Load("Sound/IA/3.Pr�sentation des videos/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationBouttonsVid�o",
            new string[]
            {
                "Vous avez la possibilit� d�acc�l�rer, de ralentir, de rembobiner ou encore de stopper une vid�o.",
                "Vous pouvez �galement zoomer sur l�un des �crans si vous jugez que votre vue est dysfonctionnelle."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/4.Pr�sentation des boutons des vid�os/1") as AudioClip,
                Resources.Load("Sound/IA/4.Pr�sentation des boutons des vid�os/2") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationRadio1",
            new string[]
            {
                "Vous ne pourrez pas comprendre tous les �v�nements qui ont eu lieu � bord du Camilla Ship en vous contentant d�analyser les vid�os.",
                "Il vous faut �galement vous servir du lecteur audio pour �couter les enregistrements r�alis�s par l��quipage.",
                "L��coute de ces enregistrements fonctionne avec les m�mes touches que celles des vid�os, c�est-�-dire stopper, acc�l�rer, ralentir et rembobiner."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/5.Pr�sentation de la radio et des cassettes/1") as AudioClip,
                Resources.Load("Sound/IA/5.Pr�sentation de la radio et des cassettes/2") as AudioClip,
                Resources.Load("Sound/IA/5.Pr�sentation de la radio et des cassettes/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationDocEcrit",
            new string[]
            {
                "Comme vous l�avez vu, vous disposez �galement de documents �crits pour vous permettre de comprendre le contexte li� � la mission de l��quipage du Camilla Ship.",
                "Contrairement aux vid�os et aux audios que nous avons pirat�s, ces documents nous ont �t� envoy�s par l��quipage avant de perdre tout contact."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/6.Pr�sentation des documents �crits/1") as AudioClip,
                Resources.Load("Sound/IA/6.Pr�sentation des documents �crits/2") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 1",
            new string[]
            {
                "A pr�sent, vous devriez commencer � vous faire une id�e de certains �v�nements qui se sont d�roul�s � bord du Camilla Ship.",
                "Je vous rappelle qu�en vertu de l�article n�5-B de la loi n�53-24, chaque rapport rempli lors d�une mission d�investigation visant � retracer des �v�nements doit inclure une frise comportant 15 points chronologiques.",
                "Etant donn� que vous n�avez pas connaissance de l�outil permettant la constitution de cette frise chronologique, je dois vous accompagner pas � pas dans sa d�couverte.",
                "Pour commencer, prenez le document �crit Journal de bord de Helena et faites le glisser jusqu�� ma Base de Donn�es Virtuelle situ�e en bas � droite de votre �cran, aussi appel�e B.D.V.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/1") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/2") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/3") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/4") as AudioClip,
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 2",
            new string[]
            {
                "Le document �crit Journal de bord de Helena est � pr�sent num�ris� dans ma B.D.V.",
                "Faites de m�me pour les documents suivants :",
                "Dernier rapport du vaisseau",
                "Note de s�curit� automatique",
                "Briefing de mission"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/5") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/6") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/7") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/8") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/9") as AudioClip,
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 3",
            new string[]
            {
                "A pr�sent, je vous invite � regarder la frise chronologique, en contrebas, sur votre bureau.",
                "Vous pouvez y observer la frise chronologique ainsi que deux panels constitu�s de cat�gories similaires :",
                "Un premier panel en haut � gauche de votre �cran appel� le P.P.C.",
                "Il contient les points chronologiques � placer sur la frise chronologique.",
                "Un deuxi�me panel en haut � droite de votre �cran, appel� P.B.D.V.",
                " Il contient les documents que vous enregistrez dans ma B.D.V, base de donn�es virtuelle.",
                "Ces documents devront �tre plac�s dans les emplacements leurs correspondant au sein des points chronologiques.",
                "Ouvrez, maintenant, la cat�gorie documents �crits, du P.P.C.",
                "Pour simplifier votre apprentissage je vais d�s lors illuminer les �l�ments auxquels je ferai r�f�rence."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/10") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/11") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/12") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/13") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/14") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/15") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/16") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/17") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/18") as AudioClip,
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 4",
            new string[]
            {
                "Cliquez sur le point chronologique.",
                "Ce dernier appara�t automatiquement sur la frise chronologique."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/19") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/20") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 5",
            new string[]
            {
                "Ouvrez, maintenant, la cat�gorie documents �crits, du P.B.D.V.",
                "Vous pouvez y observer les quatres documents pr�c�demment transmis par vos soins dans ma B.D.V."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/21") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/22") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 6",
            new string[]
            {
                "Cliquez sur le document Journal de bord de Helena et faites le glissez jusqu�� l�encart document lui �tant r�serv�.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/23") as AudioClip,
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 7",
            new string[]
            {
                "D�placez ensuite le point chronologique � l�heure concordant avec la date indiqu� dans le document Journal de bord de Helena.",
                "Pour cela il vous suffit de cliquer sur le point chronologique et de le faire glisser sur la frise chronologique jusqu�� atteindre l�heure souhait�e."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/24") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/25") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationTI 8",
            new string[]
            {
                "Puisqu�il s�agit de votre premi�re utilisation de la frise chronologique, je vous indiquerai pour chaque point chronologique si vous l�information est valide ou non.",
                "Vous n�aurez qu�� cliquer sur le bouton de validation.",
                "A pr�sent r�p�tez l�op�ration pour les trois autres documents �crits."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Pr�sentation du TI/26") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/27") as AudioClip,
                Resources.Load("Sound/IA/7.Pr�sentation du TI/28") as AudioClip
            }
        ),
        new Dialogue
        (   "Compl�tion Incorrecte",
            new string[]
            {
                "Information invalide.",
                "Merci de corriger les erreurs avant de valider ce point chronologique."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/8.Completion incorrecte tuto/1") as AudioClip,
                Resources.Load("Sound/IA/8.Completion incorrecte tuto/2") as AudioClip
            }
        ),
        new Dialogue
        (   "Compl�tion1",
            new string[]
            {
                "Information valid�e.",
                "Vous venez de compl�ter votre premier point chronologique sur la frise.",
                "Faites de m�me avec les trois autres points afin d��tablir le postulat initial de l�enqu�te."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/9.premi�re completion TI/1") as AudioClip,
                Resources.Load("Sound/IA/9.premi�re completion TI/2") as AudioClip,
                Resources.Load("Sound/IA/9.premi�re completion TI/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Compl�tion2",
            new string[]
            {
                "Information valid�e.",
                "Encore deux points chronologiques � compl�ter.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/1") as AudioClip,
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/2") as AudioClip,
            }
        ),
        new Dialogue
        (   "Compl�tion3",
            new string[]
            {
                "Information valid�e.",
                "Encore un point chronologique � compl�ter.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/3") as AudioClip,
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/4") as AudioClip,
            }
        ),
        new Dialogue
        (   "Compl�tion4",
            new string[]
            {
                "Information valid�e.",
                "F�licitations.",
                "Vous avez rempli les points chronologiques permettant d��tablir le postulat initial de l�enqu�te."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/11.4eme completion TI/1") as AudioClip,
                Resources.Load("Sound/IA/11.4eme completion TI/2") as AudioClip,
                Resources.Load("Sound/IA/11.4eme completion TI/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Transition",
            new string[]
            {
                "A pr�sent, vous n�avez plus besoin de placer de documents �crits car ils avaient d�j� �t� analys�s par mes consoeurs.",
                "Je vais ainsi effacer ces cat�gories pour vous permettre de vous concentrer davantage sur les donn�es audios et vid�os.",
                "Il a �t� port� � ma connaissance, que les humains se laissaient ais�ment distraire.",
                "En raison de mon programme, je ne peux pas vous aider davantage sur l�analyse des donn�es r�colt�es.",
                "Les liens � cr�er entre ces derni�res sont de votre ressort, enqu�trice 854-186-420.",
                "Lorsque vous penserez �tre satisfaite de votre frise chronologique vous devrez appuyer sur le bouton de validation.",
                "Le dossier d�enqu�te sera alors transmis � la commission.",
                "Concentrez-vous sur les vid�os et les audios afin de d�terminer la chronologie des �v�nements ayant eu lieu entre 15h et 15h15, le 2 septembre.",
                "Gardez en m�moire que votre enqu�te porte sur les raisons du passage de 4 � 0 signes vitaux actifs, en un lapse de temps de 15 minutes.",
                "En d�autres termes, d�couvrez pourquoi l��quipage ne donne plus signe de vie.",
                "A pr�sent, mettez-vous au travail !"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/1") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/2") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/3") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/4") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/5") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/6") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/7") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/8") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/9") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/10") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/11") as AudioClip,
                Resources.Load("Sound/IA/13.Transition vers la completion du reste du TI/mettez vous au travail 4- Transition vers completion") as AudioClip,

            }
        ),
        new Dialogue
        (   "Nebuleuse1",
            new string[]
            {
                "Attention.",
                "Une perturbation inconnue est sur le point de traverser votre navette.",
                "Cela risque d�affecter votre �quipement. Adaptez-vous en cons�quence."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/1") as AudioClip,
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/2") as AudioClip,
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationMaquette",
            new string[]
            {
                "N�h�sitez pas � consulter la maquette pour visualiser l�arriv�e de la prochaine perturbation.",
                "Vous pourrez �galement faire varier la vitesse de votre navette gr�ce au levier de pilotage."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/15.Presentation de l'axe maquette/1") as AudioClip,
                Resources.Load("Sound/IA/15.Presentation de l'axe maquette/2") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationEnergie",
            new string[]
            {
                "Gardez � l�esprit que l��nergie de votre navette se vide progressivement.",
                "Si elle arrive � court d��nergie, votre �quipement cessera de fonctionner et vous ne pourrez donc plus mener l�enqu�te.",
                "Je m�inclue �galement dans cet �quipement, alors t�chez d��tre attentif.",
                "Pour �conomiser de l��nergie, pensez � r�duire la vitesse de la navette et � �teindre les �quipements dont vous ne vous servez pas."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/16.Presentation de l'energie/1") as AudioClip,
                Resources.Load("Sound/IA/16.Presentation de l'energie/2") as AudioClip,
                Resources.Load("Sound/IA/16.Presentation de l'energie/3") as AudioClip,
                Resources.Load("Sound/IA/16.Presentation de l'energie/4") as AudioClip
            }
        ),
        new Dialogue
        (   "Pr�sentationBatterie",
            new string[]
            {
                "Vous disposez de trois batteries � �nergie thermique vous permettant de g�rer vous-m�me l��nergie de votre navette.",
                "Vous pouvez ainsi d�brancher et rebrancher la batterie connect�e � mon syst�me � tout moment.",
                "Le rechargement s�effectue en d�posant une batterie face � la fen�tre d�observation.",
                "Soyez attentif car il semble que les perturbations aient un impact sur l��nergie des batteries.",
                "N�anmoins, �tant donn� que les �tres humains sont cens�s �tre d�brouillards, vous devriez savoir vous servir de la bo�te de protection UV pour contrer certains effets n�fastes."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/17.Presentation des batteries/1") as AudioClip,
                Resources.Load("Sound/IA/17.Presentation des batteries/2") as AudioClip,
                Resources.Load("Sound/IA/17.Presentation des batteries/3") as AudioClip,
                Resources.Load("Sound/IA/17.Presentation des batteries/4") as AudioClip,
                Resources.Load("Sound/IA/17.Presentation des batteries/5") as AudioClip
            }
        ),
        new Dialogue
        (   "FinTuto",
            new string[]
            {
                "Je vous ai donn� toutes les cl�s pour r�ussir votre mission.",
                "Maintenant, c�est � vous de jouer.",
                "Je mets mes fonctions en veille, mais je vais continuer d�analyser vos progr�s et je vous avertirai de l�arriv�e potentielle de toute perturbation ou anomalie quelconque."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/1") as AudioClip,
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/2") as AudioClip,
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/3") as AudioClip
            }
        ),
        new Dialogue
        (   "N�buleuse2",
            new string[]
            {
                "Attention.",
                "Perturbation inconnue en approche."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/19.Alerte nouvelle nebuleuse/1") as AudioClip,
                Resources.Load("Sound/IA/19.Alerte nouvelle nebuleuse/2") as AudioClip,
                Resources.Load("Sound/IA/19.Alerte nouvelle nebuleuse/3") as AudioClip
            }
        ),
        new Dialogue
        (   "FinA",
            new string[]
            {
                "Communication en provenance de The Argon Station. R�ception en cours.",
                "Message du colonel Prescott : �Nous avons re�u votre rapport.",
                "Vous avez fait de l�excellent travail.",
                "Certes, ce qui est arriv� au capitaine Davis et � son �quipage est tragique, mais gr�ce � leur travail et au v�tre, une nouvelle esp�ce d��tre vivant a �t� d�couverte.",
                "Nous allons pouvoir prendre des mesures adapt�es afin d�entrer en contact avec ces parasites et les �tudier en toute s�curit� pour d�couvrir leurs secrets.",
                "Enqu�trice 854-186-420,",
                "vous avez bien servi la Stardust Conquest ainsi que toute l�humanit� qui d�pend d�elle.",
                "Vous pouvez d�sormais revenir � la Station m�re.",
                "Votre famille sera heureuse de vous retrouver saine et sauve."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/20.Fin A/1") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/2") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/3") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/4") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/5") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/6") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/7") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/8") as AudioClip,
                Resources.Load("Sound/IA/20.Fin A/9") as AudioClip
           }
        ),
        new Dialogue
        (   "FinB",
            new string[]
            {
                "Communication en provenance de The Argon Station. R�ception en cours.",
                "Message du colonel Prescott : �Nous avons re�u votre rapport. Je dois vous dire que nous sommes d��us de vous.",
                "Nos doutes quant � vos capacit�s �taient justifi�s.",
                "Nous pensions n�anmoins que si nous vous accordions une derni�re chance, vous auriez �t� mesure de prouver votre valeur.",
                "Au lieu de cela, vous nous avez envoy� un rapport falsifi� auquel il manque des �l�ments-cl�s dont nous avions d�j� connaissance avant m�me votre affectation � cette mission.",
                "De ce fait, vous �tes officiellement renvoy� de la Stardust Conquest et de son programme de colonisation spatiale.",
                "D�s que vous serez revenu � la Station m�re, votre famille et vous serez renvoy�s sur Terre de fa�on d�finitive."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/21.Fin B/1") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/2") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/3") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/4") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/5") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/6") as AudioClip,
                Resources.Load("Sound/IA/21.Fin B/7") as AudioClip
           }
        ),
        new Dialogue
        (   "Casier",
            new string[]
            {
                "Un casier conservant vos effets personnels est � votre disposition � droite de la fen�tre d�observation.",
                "Je vous invite cependant fortement � rester concentrer sur votre enqu�te et � ne pas vous �parpiller."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/22.IAMI qui nous empeche d'ouvrir le casier/1") as AudioClip,
                Resources.Load("Sound/IA/22.IAMI qui nous empeche d'ouvrir le casier/2") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto1",
            new string[]
            {
                "Vous avez compl�t� votre premier point chronologique gr�ce � vos propres d�ductions.",
                "Cependant, mon programme ne me permet pas de vous confirmer que vous avez fait les bonnes d�ductions.",
                "Je vous informerai � chaque fois que vous compl�terez un point chronologique.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/1") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/2") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/3") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto2",
            new string[]
            {
                "6�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/4") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto3",
            new string[]
            {
                "7�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/5") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto4",
            new string[]
            {
                "8�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/6") as AudioClip
           }
        ),

        new Dialogue
        (   "Compl�tionHorsTuto5",
            new string[]
            {
                "9�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/7") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto6",
            new string[]
            {
                "10�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/8") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto7",
            new string[]
            {
                "11�me point chronologique compl�t� sur 15",
                "Je tiens � vous faire remarquer que nous avons largement d�pass� la dur�e moyenne que prend une telle mission pour �tre men�e � son terme."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/9") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/10") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto8",
            new string[]
            {
                "12�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/11") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto9",
            new string[]
            {
                "13�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/12") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto10",
            new string[]
            {
                "14�me point chronologique compl�t� sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/13") as AudioClip
           }
        ),
        new Dialogue
        (   "Compl�tionHorsTuto11",
            new string[]
            {
                "Vous avez compl�t� les 15 points chronologiques n�cessaires � l'envoi de votre rapport.",
                "Si vous �tes satisfaite de votre travail, je vous demande de le valider afin que je puisse envoyer ce rapport � The Argon Station o� il sera soumis � la commission.",
                "Si vous avez encore des h�sitations, t�chez de rectifier vos erreurs avant de valider votre travail."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/14") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/15") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/16") as AudioClip
           }
        ),
        };

        RandomAntiCasierDialogue = new Dialogue[]
        {
        new Dialogue
        (   "",
            new string[]
            {
                "Concentrez-vous sur votre travail."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/1") as AudioClip,

           }
        ),
        new Dialogue
        (   "",
            new string[]
            {
                "Ce n�est pas le moment de vous d�tendre."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/2") as AudioClip,

           }
        ),
        new Dialogue
        (   "",
            new string[]
            {
                "Je vous rappelle que vous avez une enqu�te � mener."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/3") as AudioClip,

           }
        ),
        new Dialogue
        (   "",
            new string[]
            {
                "Souhaitez-vous que je fasse un rapport sur votre manque d�assiduit� ?"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/4") as AudioClip,

           }
        ),
        new Dialogue
        (   "",
            new string[]
            {
                "Il n�y a rien l�-dedans qui vous sera utile pour votre travail."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/5") as AudioClip,

           }
        ),
        new Dialogue
        (   "",
            new string[]
            {
                "Vous avez vraiment un probl�me d�attention."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/23.Pull phrases random/6") as AudioClip,

           }
        ),
        };

         source = GetComponent<AudioSource>();
        txt = GameObject.Find("IASubtitle").GetComponent<TMP_Text>();
    }

    public void LaunchDialogue(string dialogueName)
    {
        DialogueTexts = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueTexts;
        DialogueSounds = Array.Find(DialogueList, Dialogue => Dialogue.DialogueName == dialogueName).DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }

    public void LaunchRandomAntiCasierDialogue()
    {
        int iCasier = UnityEngine.Random.Range(0,6);
        DialogueTexts = RandomAntiCasierDialogue[iCasier].DialogueTexts;
        DialogueSounds = RandomAntiCasierDialogue[iCasier].DialogueSounds;
        dialogueHappening = true;
        i = 0;
        Play();
    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.A))
            //LaunchDialogue("Compl�tionHorsTuto11");

        if(dialogueHappening)
        {
            if (!source.isPlaying && !_inCooldown)
                StartCoroutine(Cooldown());
        }
    }

    public void Play()
    {
        source.clip = DialogueSounds[i];
        source.Play();
        txt.text = DialogueTexts[i];
    }
    IEnumerator Cooldown()
    {
        _inCooldown = true;
        i += 1;
        yield return new WaitForSeconds(1);
        if (i >= DialogueTexts.Length && i != 0)
        {
            DialogueTexts = null;
            DialogueSounds = null;
            dialogueHappening = false;
            txt.text = "";
        }
        else if(i != 0)
        {
            Play();
        }
        _inCooldown = false;
    }


}
