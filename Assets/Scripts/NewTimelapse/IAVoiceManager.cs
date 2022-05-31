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
                "Début de la mission INV0071-ND12-TAS.",
                "Nous sommes le 7 septembre 2246",
                "Bonjour enquêtrice 854-186-420.",
                "Je suis la nouvelle intelligence artificielle missionnée pour les investigations." ,
                "Mon nom usuel est I A M I.",
                "Veuillez vous installer rapidement à votre poste pour commencer votre mission."
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
                "Ma première tâche consiste à vous présenter ou vous rappeler comment se servir avec efficacité de l’équipement à votre disposition.",
                "Ma seconde tâche consiste à analyser votre efficacité lors de votre mission.",
                "La commission présidée par le colonel Prescott pourra ensuite étudier les résultats de votre enquête et déterminer si vous avez rempli votre mission ou si vous avez échoué.",
                "Pour commencer, lisez votre briefing de mission."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/2.Présentation de l'IA/1") as AudioClip,
                Resources.Load("Sound/IA/2.Présentation de l'IA/2") as AudioClip,
                Resources.Load("Sound/IA/2.Présentation de l'IA/3") as AudioClip,
                Resources.Load("Sound/IA/2.Présentation de l'IA/4") as AudioClip
            }
        ),
        new Dialogue
        (   "PresentationVideo",
            new string[]
            {
                "Comme vous avez pu le constater, le postulat initial de votre enquête observe un changement brutal du nombre de signes vitaux émis par l'équipage du Camilla Ship, le 2 septembre, entre 15h00 et 15h15.",
                "Avant d’aller plus loin dans la lecture d’autres documents, je vous invite à regarder les vidéos de surveillance du Camilla Ship.",
                "Malheureusement, il n’y a pas de sons sur ces vidéos : le résultat d’un manque d’entretien, typiquement humain.",
                "Vous allez devoir vous débrouiller sans."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/2.Présentation de l'IA/5") as AudioClip,
                Resources.Load("Sound/IA/3.Présentation des videos/1") as AudioClip,
                Resources.Load("Sound/IA/3.Présentation des videos/2") as AudioClip,
                Resources.Load("Sound/IA/3.Présentation des videos/3") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationBouttonsVidéo",
            new string[]
            {
                "Vous avez la possibilité d’accélérer, de ralentir, de rembobiner ou encore de stopper une vidéo.",
                "Vous pouvez également zoomer sur l’un des écrans si vous jugez que votre vue est dysfonctionnelle."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/4.Présentation des boutons des vidéos/1") as AudioClip,
                Resources.Load("Sound/IA/4.Présentation des boutons des vidéos/2") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationRadio1",
            new string[]
            {
                "Vous ne pourrez pas comprendre tous les événements qui ont eu lieu à bord du Camilla Ship en vous contentant d’analyser les vidéos.",
                "Il vous faut également vous servir du lecteur audio pour écouter les enregistrements réalisés par l’équipage.",
                "L’écoute de ces enregistrements fonctionne avec les mêmes touches que celles des vidéos, c’est-à-dire stopper, accélérer, ralentir et rembobiner."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/5.Présentation de la radio et des cassettes/1") as AudioClip,
                Resources.Load("Sound/IA/5.Présentation de la radio et des cassettes/2") as AudioClip,
                Resources.Load("Sound/IA/5.Présentation de la radio et des cassettes/3") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationDocEcrit",
            new string[]
            {
                "Comme vous l’avez vu, vous disposez également de documents écrits pour vous permettre de comprendre le contexte lié à la mission de l’équipage du Camilla Ship.",
                "Contrairement aux vidéos et aux audios que nous avons piratés, ces documents nous ont été envoyés par l’équipage avant de perdre tout contact."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/6.Présentation des documents écrits/1") as AudioClip,
                Resources.Load("Sound/IA/6.Présentation des documents écrits/2") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationTI 1",
            new string[]
            {
                "A présent, vous devriez commencer à vous faire une idée de certains événements qui se sont déroulés à bord du Camilla Ship.",
                "Je vous rappelle qu’en vertu de l’article n°5-B de la loi n°53-24, chaque rapport rempli lors d’une mission d’investigation visant à retracer des événements doit inclure une frise comportant 15 points chronologiques.",
                "Etant donné que vous n’avez pas connaissance de l’outil permettant la constitution de cette frise chronologique, je dois vous accompagner pas à pas dans sa découverte.",
                "Pour commencer, prenez le document écrit Journal de bord de Helena et faites le glisser jusqu’à ma Base de Données Virtuelle située en bas à droite de votre écran, aussi appelée B.D.V.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/1") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/2") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/3") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/4") as AudioClip,
            }
        ),
        new Dialogue
        (   "PrésentationTI 2",
            new string[]
            {
                "Le document écrit Journal de bord de Helena est à présent numérisé dans ma B.D.V.",
                "Faites de même pour les documents suivants :",
                "Dernier rapport du vaisseau",
                "Note de sécurité automatique",
                "Briefing de mission"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/5") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/6") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/7") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/8") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/9") as AudioClip,
            }
        ),
        new Dialogue
        (   "PrésentationTI 3",
            new string[]
            {
                "A présent, je vous invite à regarder la frise chronologique, en contrebas, sur votre bureau.",
                "Vous pouvez y observer la frise chronologique ainsi que deux panels constitués de catégories similaires :",
                "Un premier panel en haut à gauche de votre écran appelé le P.P.C.",
                "Il contient les points chronologiques à placer sur la frise chronologique.",
                "Un deuxième panel en haut à droite de votre écran, appelé P.B.D.V.",
                " Il contient les documents que vous enregistrez dans ma B.D.V, base de données virtuelle.",
                "Ces documents devront être placés dans les emplacements leurs correspondant au sein des points chronologiques.",
                "Ouvrez, maintenant, la catégorie documents écrits, du P.P.C.",
                "Pour simplifier votre apprentissage je vais dès lors illuminer les éléments auxquels je ferai référence."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/10") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/11") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/12") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/13") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/14") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/15") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/16") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/17") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/18") as AudioClip,
            }
        ),
        new Dialogue
        (   "PrésentationTI 4",
            new string[]
            {
                "Cliquez sur le point chronologique.",
                "Ce dernier apparaît automatiquement sur la frise chronologique."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/19") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/20") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationTI 5",
            new string[]
            {
                "Ouvrez, maintenant, la catégorie documents écrits, du P.B.D.V.",
                "Vous pouvez y observer les quatres documents précédemment transmis par vos soins dans ma B.D.V."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/21") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/22") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationTI 6",
            new string[]
            {
                "Cliquez sur le document Journal de bord de Helena et faites le glissez jusqu’à l’encart document lui étant réservé.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/23") as AudioClip,
            }
        ),
        new Dialogue
        (   "PrésentationTI 7",
            new string[]
            {
                "Déplacez ensuite le point chronologique à l’heure concordant avec la date indiqué dans le document Journal de bord de Helena.",
                "Pour cela il vous suffit de cliquer sur le point chronologique et de le faire glisser sur la frise chronologique jusqu’à atteindre l’heure souhaitée."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/24") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/25") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationTI 8",
            new string[]
            {
                "Puisqu’il s’agit de votre première utilisation de la frise chronologique, je vous indiquerai pour chaque point chronologique si vous l’information est valide ou non.",
                "Vous n’aurez qu’à cliquer sur le bouton de validation.",
                "A présent répétez l’opération pour les trois autres documents écrits."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/7.Présentation du TI/26") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/27") as AudioClip,
                Resources.Load("Sound/IA/7.Présentation du TI/28") as AudioClip
            }
        ),
        new Dialogue
        (   "Complétion Incorrecte",
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
        (   "Complétion1",
            new string[]
            {
                "Information validée.",
                "Vous venez de compléter votre premier point chronologique sur la frise.",
                "Faites de même avec les trois autres points afin d’établir le postulat initial de l’enquête."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/9.première completion TI/1") as AudioClip,
                Resources.Load("Sound/IA/9.première completion TI/2") as AudioClip,
                Resources.Load("Sound/IA/9.première completion TI/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Complétion2",
            new string[]
            {
                "Information validée.",
                "Encore deux points chronologiques à compléter.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/1") as AudioClip,
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/2") as AudioClip,
            }
        ),
        new Dialogue
        (   "Complétion3",
            new string[]
            {
                "Information validée.",
                "Encore un point chronologique à compléter.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/3") as AudioClip,
                Resources.Load("Sound/IA/10.2eme et 3eme completion TI/4") as AudioClip,
            }
        ),
        new Dialogue
        (   "Complétion4",
            new string[]
            {
                "Information validée.",
                "Félicitations.",
                "Vous avez rempli les points chronologiques permettant d’établir le postulat initial de l’enquête."
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
                "A présent, vous n’avez plus besoin de placer de documents écrits car ils avaient déjà été analysés par mes consoeurs.",
                "Je vais ainsi effacer ces catégories pour vous permettre de vous concentrer davantage sur les données audios et vidéos.",
                "Il a été porté à ma connaissance, que les humains se laissaient aisément distraire.",
                "En raison de mon programme, je ne peux pas vous aider davantage sur l’analyse des données récoltées.",
                "Les liens à créer entre ces dernières sont de votre ressort, enquêtrice 854-186-420.",
                "Lorsque vous penserez être satisfaite de votre frise chronologique vous devrez appuyer sur le bouton de validation.",
                "Le dossier d’enquête sera alors transmis à la commission.",
                "Concentrez-vous sur les vidéos et les audios afin de déterminer la chronologie des événements ayant eu lieu entre 15h et 15h15, le 2 septembre.",
                "Gardez en mémoire que votre enquête porte sur les raisons du passage de 4 à 0 signes vitaux actifs, en un lapse de temps de 15 minutes.",
                "En d’autres termes, découvrez pourquoi l’équipage ne donne plus signe de vie.",
                "A présent, mettez-vous au travail !"
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
                "Cela risque d’affecter votre équipement. Adaptez-vous en conséquence."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/1") as AudioClip,
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/2") as AudioClip,
                Resources.Load("Sound/IA/14.Alerte 1ere nebuleuse/3") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationMaquette",
            new string[]
            {
                "N’hésitez pas à consulter la maquette pour visualiser l’arrivée de la prochaine perturbation.",
                "Vous pourrez également faire varier la vitesse de votre navette grâce au levier de pilotage."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/15.Presentation de l'axe maquette/1") as AudioClip,
                Resources.Load("Sound/IA/15.Presentation de l'axe maquette/2") as AudioClip
            }
        ),
        new Dialogue
        (   "PrésentationEnergie",
            new string[]
            {
                "Gardez à l’esprit que l’énergie de votre navette se vide progressivement.",
                "Si elle arrive à court d’énergie, votre équipement cessera de fonctionner et vous ne pourrez donc plus mener l’enquête.",
                "Je m’inclue également dans cet équipement, alors tâchez d’être attentif.",
                "Pour économiser de l’énergie, pensez à réduire la vitesse de la navette et à éteindre les équipements dont vous ne vous servez pas."
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
        (   "PrésentationBatterie",
            new string[]
            {
                "Vous disposez de trois batteries à énergie thermique vous permettant de gérer vous-même l’énergie de votre navette.",
                "Vous pouvez ainsi débrancher et rebrancher la batterie connectée à mon système à tout moment.",
                "Le rechargement s’effectue en déposant une batterie face à la fenêtre d’observation.",
                "Soyez attentif car il semble que les perturbations aient un impact sur l’énergie des batteries.",
                "Néanmoins, étant donné que les êtres humains sont censés être débrouillards, vous devriez savoir vous servir de la boîte de protection UV pour contrer certains effets néfastes."
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
                "Je vous ai donné toutes les clés pour réussir votre mission.",
                "Maintenant, c’est à vous de jouer.",
                "Je mets mes fonctions en veille, mais je vais continuer d’analyser vos progrès et je vous avertirai de l’arrivée potentielle de toute perturbation ou anomalie quelconque."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/1") as AudioClip,
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/2") as AudioClip,
                Resources.Load("Sound/IA/18.Fin du tuto mise en veille/3") as AudioClip
            }
        ),
        new Dialogue
        (   "Nébuleuse2",
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
                "Communication en provenance de The Argon Station. Réception en cours.",
                "Message du colonel Prescott : “Nous avons reçu votre rapport.",
                "Vous avez fait de l’excellent travail.",
                "Certes, ce qui est arrivé au capitaine Davis et à son équipage est tragique, mais grâce à leur travail et au vôtre, une nouvelle espèce d’être vivant a été découverte.",
                "Nous allons pouvoir prendre des mesures adaptées afin d’entrer en contact avec ces parasites et les étudier en toute sécurité pour découvrir leurs secrets.",
                "Enquêtrice 854-186-420,",
                "vous avez bien servi la Stardust Conquest ainsi que toute l’humanité qui dépend d’elle.",
                "Vous pouvez désormais revenir à la Station mère.",
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
                "Communication en provenance de The Argon Station. Réception en cours.",
                "Message du colonel Prescott : “Nous avons reçu votre rapport. Je dois vous dire que nous sommes déçus de vous.",
                "Nos doutes quant à vos capacités étaient justifiés.",
                "Nous pensions néanmoins que si nous vous accordions une dernière chance, vous auriez été mesure de prouver votre valeur.",
                "Au lieu de cela, vous nous avez envoyé un rapport falsifié auquel il manque des éléments-clés dont nous avions déjà connaissance avant même votre affectation à cette mission.",
                "De ce fait, vous êtes officiellement renvoyé de la Stardust Conquest et de son programme de colonisation spatiale.",
                "Dès que vous serez revenu à la Station mère, votre famille et vous serez renvoyés sur Terre de façon définitive."
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
                "Un casier conservant vos effets personnels est à votre disposition à droite de la fenêtre d’observation.",
                "Je vous invite cependant fortement à rester concentrer sur votre enquête et à ne pas vous éparpiller."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/22.IAMI qui nous empeche d'ouvrir le casier/1") as AudioClip,
                Resources.Load("Sound/IA/22.IAMI qui nous empeche d'ouvrir le casier/2") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto1",
            new string[]
            {
                "Vous avez complété votre premier point chronologique grâce à vos propres déductions.",
                "Cependant, mon programme ne me permet pas de vous confirmer que vous avez fait les bonnes déductions.",
                "Je vous informerai à chaque fois que vous compléterez un point chronologique.",
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/1") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/2") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/3") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto2",
            new string[]
            {
                "6ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/4") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto3",
            new string[]
            {
                "7ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/5") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto4",
            new string[]
            {
                "8ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/6") as AudioClip
           }
        ),

        new Dialogue
        (   "ComplétionHorsTuto5",
            new string[]
            {
                "9ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/7") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto6",
            new string[]
            {
                "10ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/8") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto7",
            new string[]
            {
                "11ème point chronologique complété sur 15",
                "Je tiens à vous faire remarquer que nous avons largement dépassé la durée moyenne que prend une telle mission pour être menée à son terme."
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/9") as AudioClip,
                Resources.Load("Sound/IA/24.completion hors tuto/10") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto8",
            new string[]
            {
                "12ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/11") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto9",
            new string[]
            {
                "13ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/12") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto10",
            new string[]
            {
                "14ème point chronologique complété sur 15"
            },
            new AudioClip[]
            {
                Resources.Load("Sound/IA/24.completion hors tuto/13") as AudioClip
           }
        ),
        new Dialogue
        (   "ComplétionHorsTuto11",
            new string[]
            {
                "Vous avez complété les 15 points chronologiques nécessaires à l'envoi de votre rapport.",
                "Si vous êtes satisfaite de votre travail, je vous demande de le valider afin que je puisse envoyer ce rapport à The Argon Station où il sera soumis à la commission.",
                "Si vous avez encore des hésitations, tâchez de rectifier vos erreurs avant de valider votre travail."
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
                "Ce n’est pas le moment de vous détendre."
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
                "Je vous rappelle que vous avez une enquête à mener."
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
                "Souhaitez-vous que je fasse un rapport sur votre manque d’assiduité ?"
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
                "Il n’y a rien là-dedans qui vous sera utile pour votre travail."
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
                "Vous avez vraiment un problème d’attention."
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
            //LaunchDialogue("ComplétionHorsTuto11");

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
