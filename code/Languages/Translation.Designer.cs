﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace notifier.Languages {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Translation {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Translation() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("notifier.Languages.Translation", typeof(Translation).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Fermeture de l&apos;application.
        /// </summary>
        internal static string applicationExit {
            get {
                return ResourceManager.GetString("applicationExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Vous êtes sur le point de quitter l&apos;application.
        ///
        ///Voulez-vous vraiment quitter l&apos;application ?.
        /// </summary>
        internal static string applicationExitQuestion {
            get {
                return ResourceManager.GetString("applicationExitQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Erreur d&apos;authentification.
        /// </summary>
        internal static string authenticationFailed {
            get {
                return ResourceManager.GetString("authenticationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Vous avez refusé que l&apos;application accède à votre compte Gmail. Cette étape est nécessaire et vous sera demandée à nouveau lors du prochain démarrage.
        ///
        ///L&apos;application va désormais quitter..
        /// </summary>
        internal static string authenticationWithGmailRefused {
            get {
                return ResourceManager.GetString("authenticationWithGmailRefused", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Vérifier les mises à jour.
        /// </summary>
        internal static string checkForUpdate {
            get {
                return ResourceManager.GetString("checkForUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Erreur.
        /// </summary>
        internal static string error {
            get {
                return ResourceManager.GetString("error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Gmail notifier s&apos;exécute désormais en arrière plan sur votre ordinateur. Dans quelques secondes, vous serez invité à vous connecter sur votre compte Gmail afin d&apos;autoriser l&apos;application à accéder à votre boite de réception..
        /// </summary>
        internal static string firstLoad {
            get {
                return ResourceManager.GetString("firstLoad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Certifié sans spyware, sans adware et sans virus.
        /// </summary>
        internal static string freeSoftware {
            get {
                return ResourceManager.GetString("freeSoftware", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Déconnexion de votre compte Gmail.
        /// </summary>
        internal static string gmailDisconnect {
            get {
                return ResourceManager.GetString("gmailDisconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Vous êtes sur le point de déconnecter votre compte Gmail de l&apos;application : vous ne recevrez plus de notifications en provenance du compte {account_name} et vous serez invité à vous reconnecter au prochain démarrage.
        ///
        ///Voulez-vous vraiment déconnecter votre compte Google et redémarrer l&apos;application maintenant ?.
        /// </summary>
        internal static string gmailDisconnectQuestion {
            get {
                return ResourceManager.GetString("gmailDisconnectQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Pose une question avant de quitter totalement l&apos;application.
        /// </summary>
        internal static string helpAskonExit {
            get {
                return ResourceManager.GetString("helpAskonExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche une notification lorsque l&apos;application tente de se reconnecter au réseau.
        /// </summary>
        internal static string helpAttemptToReconnectNotification {
            get {
                return ResourceManager.GetString("helpAttemptToReconnectNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Joue un son lors de l&apos;affichage d&apos;une notification.
        /// </summary>
        internal static string helpAudioNotification {
            get {
                return ResourceManager.GetString("helpAudioNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Compte Gmail actuellement connecté à l&apos;application.
        /// </summary>
        internal static string helpEmailAddress {
            get {
                return ResourceManager.GetString("helpEmailAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Déconnecte le compte Gmail et redémarre l&apos;application pour demander une nouvelle authentification.
        /// </summary>
        internal static string helpGmailDisconnect {
            get {
                return ResourceManager.GetString("helpGmailDisconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Langue de l&apos;interface.
        /// </summary>
        internal static string helpLanguage {
            get {
                return ResourceManager.GetString("helpLanguage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche une notification lors de la réception de nouveaux messages.
        /// </summary>
        internal static string helpMessageNotification {
            get {
                return ResourceManager.GetString("helpMessageNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Action réalisée lors d&apos;un clic sur une notification.
        /// </summary>
        internal static string helpNotificationBehavior {
            get {
                return ResourceManager.GetString("helpNotificationBehavior", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Délai de synchronisation de la boite de réception.
        /// </summary>
        internal static string helpNumericDelay {
            get {
                return ResourceManager.GetString("helpNumericDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche une notification ne contenant aucune informations sur le message.
        /// </summary>
        internal static string helpPrivacyNotificationAll {
            get {
                return ResourceManager.GetString("helpPrivacyNotificationAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche le nom de l&apos;expéditeur ainsi que le contenu du message.
        /// </summary>
        internal static string helpPrivacyNotificationNone {
            get {
                return ResourceManager.GetString("helpPrivacyNotificationNone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche le nom de l&apos;expéditeur et l&apos;objet du message.
        /// </summary>
        internal static string helpPrivacyNotificationShort {
            get {
                return ResourceManager.GetString("helpPrivacyNotificationShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Lance l&apos;application au démarrage de Windows.
        /// </summary>
        internal static string helpRunAtWindowsStartup {
            get {
                return ResourceManager.GetString("helpRunAtWindowsStartup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Affiche une notification lors de la réception de courrier indésirable.
        /// </summary>
        internal static string helpSpamNotification {
            get {
                return ResourceManager.GetString("helpSpamNotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Échelle de temps pour la synchronisation de la boite de réception.
        /// </summary>
        internal static string helpStepDelay {
            get {
                return ResourceManager.GetString("helpStepDelay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Date de dernière mise à jour du jeton de validité de l&apos;authentification.
        /// </summary>
        internal static string helpTokenDelivery {
            get {
                return ResourceManager.GetString("helpTokenDelivery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Vous disposez de la dernière version de l&apos;application..
        /// </summary>
        internal static string latestVersion {
            get {
                return ResourceManager.GetString("latestVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Marquer comme lu.
        /// </summary>
        internal static string markAsRead {
            get {
                return ResourceManager.GetString("markAsRead", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Erreur lors de l&apos;opération &quot;Marquer comme lu&quot;.
        /// </summary>
        internal static string markAsReadError {
            get {
                return ResourceManager.GetString("markAsReadError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Une erreur est survenue lors de l&apos;opération &quot;Marquer comme lu&quot; :.
        /// </summary>
        internal static string markAsReadErrorOccured {
            get {
                return ResourceManager.GetString("markAsReadErrorOccured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Instances multiples.
        /// </summary>
        internal static string multipleInstances {
            get {
                return ResourceManager.GetString("multipleInstances", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à L&apos;application &quot;Gmail notifier&quot; est déjà en cours d&apos;exécution : vous ne pouvez pas lancer plusieurs instances de l&apos;application sur un même ordinateur.
        ///
        ///Cette option n&apos;est pas activée sur ce type d&apos;application..
        /// </summary>
        internal static string mutexError {
            get {
                return ResourceManager.GetString("mutexError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Double-cliquez sur l&apos;icône pour accéder à votre boîte de réception..
        /// </summary>
        internal static string newUnreadMessage {
            get {
                return ResourceManager.GetString("newUnreadMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Double-cliquez sur l&apos;icône pour accéder à votre boîte de réception. Tant que vous n&apos;aurez pas vérifié le dossier spam, votre boite de réception ne sera pas synchronisée et vous ne serez pas informé de l&apos;arrivée de nouveaux messages..
        /// </summary>
        internal static string newUnreadSpam {
            get {
                return ResourceManager.GetString("newUnreadSpam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Une version plus récente de l&apos;application est disponible sur Github :
        ///Gmail Notifier {version}.
        ///
        ///Voulez-vous la télécharger maintenant ?.
        /// </summary>
        internal static string newVersion {
            get {
                return ResourceManager.GetString("newVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Pas de nouveau message.
        /// </summary>
        internal static string noMessage {
            get {
                return ResourceManager.GetString("noMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Tentative de reconnexion ....
        /// </summary>
        internal static string reconnectAttempt {
            get {
                return ResourceManager.GetString("reconnectAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à La reconnexion a échoué.
        /// </summary>
        internal static string reconnectFailed {
            get {
                return ResourceManager.GetString("reconnectFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Synchronisation en cours ....
        /// </summary>
        internal static string sync {
            get {
                return ResourceManager.GetString("sync", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Erreur lors de la synchronisation.
        /// </summary>
        internal static string syncError {
            get {
                return ResourceManager.GetString("syncError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Une erreur est survenue lors de la synchronisation de la boite de réception :.
        /// </summary>
        internal static string syncErrorOccured {
            get {
                return ResourceManager.GetString("syncErrorOccured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Mis à jour à {time}.
        /// </summary>
        internal static string syncTime {
            get {
                return ResourceManager.GetString("syncTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Ne pas déranger.
        /// </summary>
        internal static string timeout {
            get {
                return ResourceManager.GetString("timeout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Notes de version Github.
        /// </summary>
        internal static string tipReleaseNotes {
            get {
                return ResourceManager.GetString("tipReleaseNotes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Softpedia, 100% gratuit.
        /// </summary>
        internal static string tipSoftpedia {
            get {
                return ResourceManager.GetString("tipSoftpedia", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à email non lu.
        /// </summary>
        internal static string unreadMessage {
            get {
                return ResourceManager.GetString("unreadMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à emails non lus.
        /// </summary>
        internal static string unreadMessages {
            get {
                return ResourceManager.GetString("unreadMessages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à spam non lu.
        /// </summary>
        internal static string unreadSpam {
            get {
                return ResourceManager.GetString("unreadSpam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à spams non lus.
        /// </summary>
        internal static string unreadSpams {
            get {
                return ResourceManager.GetString("unreadSpams", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Le service de mise à jour est inaccessible pour le moment..
        /// </summary>
        internal static string updateServiceUnreachable {
            get {
                return ResourceManager.GetString("updateServiceUnreachable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Téléchargement de la mise à jour ....
        /// </summary>
        internal static string updating {
            get {
                return ResourceManager.GetString("updating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Bienvenue !.
        /// </summary>
        internal static string welcome {
            get {
                return ResourceManager.GetString("welcome", resourceCulture);
            }
        }
    }
}