// wwwroot/js/planStorage.js

window.PlanStorage = {
    // Fetch all completed videos from localStorage
    getCompletedVideos: function () {
        let data = localStorage.getItem("completedVideos");
        return data ? JSON.parse(data) : {};
    },

    // Save all completed videos to localStorage
    saveCompletedVideos: function (data) {
        localStorage.setItem("completedVideos", JSON.stringify(data));
    },

    // Mark a video as completed for a specific plan
    markVideoCompleted: function (planId, videoId) {
        let completed = this.getCompletedVideos();

        if (!completed[planId]) completed[planId] = new Set();
        else completed[planId] = new Set(completed[planId]);

        completed[planId].add(videoId);

        // Convert Set back to Array for storage
        completed[planId] = Array.from(completed[planId]);
        this.saveCompletedVideos(completed);
    },

    // Check if a video is completed for a specific plan
    isVideoCompleted: function (planId, videoId) {
        let completed = this.getCompletedVideos();
        return completed[planId] ? completed[planId].includes(videoId) : false;
    },

    // Get progress for a specific plan
    getPlanProgress: function (planId, totalLessons) {
        let completed = this.getCompletedVideos();
        let count = completed[planId] ? completed[planId].length : 0;
        return { completed: count, total: totalLessons };
    },

    // Reset (clear) completed videos for a specific plan
    resetPlan: function (planId) {
        let completed = this.getCompletedVideos();
        if (completed[planId]) {
            delete completed[planId];
            this.saveCompletedVideos(completed);
        }
    }
};
